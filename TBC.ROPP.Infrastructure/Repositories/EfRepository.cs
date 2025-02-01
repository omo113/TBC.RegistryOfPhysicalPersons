using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TBC.ROPP.Domain.Abstractions;
using TBC.ROPP.Domain.Enums;
using TBC.ROPP.Infrastructure.Persistance;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;

namespace TBC.ROPP.Infrastructure.Repositories
{
    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        private readonly ApplicationDbContext _db;
        private readonly IQueryable<TEntity> _baseQuery;

        public EfRepository(ApplicationDbContext db)
        {
            _db = db;
            _baseQuery = _db.Set<TEntity>().AsQueryable();
        }

        public virtual async Task<TEntity?> Find(int uId, bool onlyActive = true)
        {
            return onlyActive
                       ? await _baseQuery.Where(x => x.EntityStatus == EntityStatus.Active).SingleOrDefaultAsync(x => x.Id == uId)
                       : await _baseQuery.SingleOrDefaultAsync(x => x.Id == uId);
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? expression = null, bool onlyActives = true)
        {
            var query = onlyActives
                            ? _baseQuery.Where(x => x.EntityStatus == EntityStatus.Active)
                            : _baseQuery;

            return expression == null ? query : query.Where(expression);
        }

        public virtual async Task Store(TEntity document)
        {
            await _db.Set<TEntity>().AddAsync(document);
        }
    }
}