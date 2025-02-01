using TBC.ROPP.Infrastructure.Persistance.Entities;

namespace TBC.ROPP.MigrationClient.Abstractions;

internal abstract class PreMigrationClientScript : IMigrationClientScript
{
    public MigrationClientScriptTypes Type => MigrationClientScriptTypes.PreEfMigration;
    public abstract Task RunAsync(CancellationToken cancellationToken);
}