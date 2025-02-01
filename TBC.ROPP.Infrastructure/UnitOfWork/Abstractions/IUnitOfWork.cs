namespace TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}