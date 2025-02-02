using Microsoft.AspNetCore.Identity;
using TBC.ROPP.Domain.IdentityEntities;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;
using TBC.ROPP.MigrationClient.Abstractions;
using TBC.ROPP.MigrationClient.Shared;

namespace TBC.ROPP.MigrationClient.Scripts;

public class InitMigration(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IUnitOfWork unitOfWork)
    : PostMigrationClientScript
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public override async Task RunAsync(CancellationToken cancellationToken)
    {
        var roleResult = await roleManager.CreateAsync(new ApplicationRole
        {
            Name = "Administrator"
        });
        if (!roleResult.Succeeded)
        {
            throw new MigrationScriptFailedException(nameof(InitMigration), string.Concat(roleResult.Errors.Select(x => $"{x.Code} - {x.Description}")));
        }

        var userResult = await userManager.CreateAsync(ApplicationUser.Create("admin", "admin", "admin"), "Admin123!");
        if (!userResult.Succeeded)
        {
            throw new MigrationScriptFailedException(nameof(InitMigration), string.Concat(userResult.Errors.Select(x => $"{x.Code} - {x.Description}")));
        }

        var user = await userManager.FindByNameAsync("admin");
        if (user == null)
        {
            throw new MigrationScriptFailedException(nameof(InitMigration));
        }

        userResult = await userManager.AddToRoleAsync(user, "Administrator");
        if (!userResult.Succeeded)
        {
            throw new MigrationScriptFailedException(nameof(InitMigration), string.Concat(userResult.Errors.Select(x => $"{x.Code} - {x.Description}")));
        }
    }
}