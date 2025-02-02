using Microsoft.AspNetCore.Identity;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.ValueObjects;
using TBC.ROPP.Domain.IdentityEntities;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;
using TBC.ROPP.MigrationClient.Abstractions;
using TBC.ROPP.MigrationClient.Shared;

namespace TBC.ROPP.MigrationClient.Scripts;

public class InitMigration(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IUnitOfWork unitOfWork,
    IRepository<PhysicalPerson> repository)
    : PostMigrationClientScript
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly List<string> _names = ["giorgi", "pavle", "mariami", "nino", "gvanca", "levani", "jimsheri", "ramesaxeli", "rumesaxeli"];
    private readonly List<string> _lastName = ["giorgi", "pavle", "mariami", "nino", "gvanca", "levani", "jimsheri", "ramesaxeli", "rumesaxeli"];
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

        var people = Enumerable.Range(0, 1000).Select(x => PhysicalPerson.Create(
            _names[Random.Shared.Next(_names.Count)],
            _names[Random.Shared.Next(_names.Count)],
            Random.Shared.Next(100) >= 50 ? Gender.Female : Gender.Male,
            $"{10_000_000_000 + x}",
            DateTimeOffset.Now,
            1,
            new List<PhoneNumber>()
            {
                PhoneNumber.Create(PhoneNumberType.Mobile,"123123213123")
            }));
        foreach (var person in people)
        {
            await repository.Store(person);
        }

        await unitOfWork.SaveAsync(cancellationToken);
    }
}