using Microsoft.Extensions.DependencyInjection;
using TBC.ROPP.MigrationClient.Abstractions;
using TBC.ROPP.MigrationClient.Scripts;

namespace TBC.ROPP.MigrationClient;

public static class DIExtensions
{
    public static IServiceCollection AddMigrations(this IServiceCollection services)
    {
        services.AddHostedService<MigrationService>();
        services.AddScoped<IMigrationClientScript, InitMigration>();
        return services;
    }
}