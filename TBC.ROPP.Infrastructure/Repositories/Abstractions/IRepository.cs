using System.Linq.Expressions;

namespace TBC.ROPP.Infrastructure.Repositories.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<T?> Find(int uId, bool onlyActive = true);

        IQueryable<T> Query(Expression<Func<T, bool>>? expression = null, bool onlyActives = true);

        Task Store(T document);
    }
}