using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TBC.ROPP.Domain.IdentityEntities;
using TBC.ROPP.Infrastructure.FileStorage.Abstractions;
using TBC.ROPP.Infrastructure.Persistance;
using TBC.ROPP.Infrastructure.Repositories;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;
using TBC.ROPP.Shared;

namespace TBC.ROPP.Infrastructure;

public static class DIExtension
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var applicationSettings = sp.GetRequiredService<IOptions<ApplicationSettings>>();
            options.UseSqlServer(applicationSettings.Value.DatabaseConnection);
        });
        services.AddIdentityCore<ApplicationUser>(options =>
        {
        })
        .AddRoles<ApplicationRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddFileInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IFileStorage, FileStorage.FileStorage>();
        return services;
    }
}