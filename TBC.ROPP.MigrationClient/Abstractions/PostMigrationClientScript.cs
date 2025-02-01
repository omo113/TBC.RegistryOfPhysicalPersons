using TBC.ROPP.Infrastructure.Persistance.Entities;

namespace TBC.ROPP.MigrationClient.Abstractions;

public abstract class PostMigrationClientScript : IMigrationClientScript
{
    public MigrationClientScriptTypes Type => MigrationClientScriptTypes.PostEfMigration;

    public abstract Task RunAsync(CancellationToken cancellationToken);
}