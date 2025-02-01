using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TBC.ROPP.Infrastructure.Persistance.Entities;

namespace TBC.ROPP.Infrastructure.Persistance.Abstractions;

public interface IMigrationDbContext
{
    /// <summary>
    ///     Provides access to database related information and operations for this context.
    /// </summary>
    public DatabaseFacade Database { get; }

    public DbSet<MigrationClientScript> MigrationClientScripts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
}