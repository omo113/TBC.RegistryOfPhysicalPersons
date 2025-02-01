using TBC.ROPP.Infrastructure.Persistance.Entities;

namespace TBC.ROPP.MigrationClient.Abstractions;

internal interface IMigrationClientScript
{
    MigrationClientScriptTypes Type { get; }
    Task RunAsync(CancellationToken cancellationToken);
}