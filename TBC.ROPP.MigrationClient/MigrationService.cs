using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using TBC.ROPP.Infrastructure.Persistance;
using TBC.ROPP.Infrastructure.Persistance.Abstractions;
using TBC.ROPP.Infrastructure.Persistance.Entities;
using TBC.ROPP.MigrationClient.Abstractions;
using TBC.ROPP.Shared;

namespace TBC.ROPP.MigrationClient
{
    public class MigrationService : IHostedService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MigrationService> _logger;
        private readonly bool _standaloneService;

        public MigrationService(IHostEnvironment host,
                                ILogger<MigrationService> logger,
                                IHostApplicationLifetime hostApplicationLifetime,
                                IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
            _scopeFactory = scopeFactory;

            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            _standaloneService = string.IsNullOrEmpty(assemblyName) || host.ApplicationName == assemblyName;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return ExecuteAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await RunMigration(stoppingToken);
                if (_standaloneService)
                {
                    _hostApplicationLifetime.StopApplication();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                if (!_standaloneService)
                {
                    throw;
                }
            }
        }

        private async Task RunMigration(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<MigrationService>>();

            var migrationContext = scope.ServiceProvider.GetRequiredService<IMigrationDbContext>();

            await RunContextMigration(migrationContext.Database, cancellationToken);
            await RunContextMigration(scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database, cancellationToken);

            var scripts = await migrationContext.MigrationClientScripts.ToListAsync(cancellationToken);

            var appliedMigrations = await migrationContext.Database.GetAppliedMigrationsAsync(cancellationToken);
            var migrations = appliedMigrations.ToList();

            foreach (var item in scope.ServiceProvider.GetServices<IMigrationClientScript>()
                                      .Where(x => x.Type == MigrationClientScriptTypes.PostEfMigration)
                                      .Where(x => scripts.All(c => c.Script != x.GetType().Name)))
            {
                logger.LogInformation("--- Seeding {Name} ---", item.GetType().Name);
                await item.RunAsync(cancellationToken);

                migrationContext.MigrationClientScripts.Add(new MigrationClientScript
                {
                    Script = item.GetType().Name,
                    Type = item.Type,
                    EfMigration = migrations.LastOrDefault() ?? string.Empty,
                    Date = SystemDate.Now.UtcDateTime
                });
                await migrationContext.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task RunContextMigration(DatabaseFacade database, CancellationToken cancellationToken)
        {
            var pendingMigrations = await database.GetPendingMigrationsAsync(cancellationToken);
            if (pendingMigrations.Any())
            {
                await database.MigrateAsync(cancellationToken);
            }
        }
    }
}