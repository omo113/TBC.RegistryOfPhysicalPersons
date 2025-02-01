using TBC.ROPP.Domain.Shared;
using TBC.ROPP.Infrastructure.Persistance;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;

namespace TBC.ROPP.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {

        var events = _dbContext.ChangeTracker.Entries<IHasDomainEvent>()
            .Select(x => x.Entity.PendingDomainEvents())
            .SelectMany(x => x)
            .ToList();

        if (events.Count > 0)
        {
            var eventLogs = events.Select(x => new EventLog(x.GetType().Name,
                    x.UId.ToString(),
                    x))
                .ToList();


            await _dbContext.EventLogs.AddRangeAsync(eventLogs, cancellationToken);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}