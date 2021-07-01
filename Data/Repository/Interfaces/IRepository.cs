using Data.Tools.Implementation;
using Data.Tools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    public interface IRepository<TEntity, TKey> where TEntity : class, IDatabaseItem<TKey>
    {
        public IEnumerable<TEntity> SearchAndOrderAsync(Expression<Func<TEntity, bool>> predicate, GenericSortOptions<TEntity> sortOptions, int numRecords);
        public Task<IEnumerable<TEntity>> SearchAllAsync(Expression<Func<TEntity, bool>> predicate);
        public IEnumerable<TEntity> SearchAndOrder(IQueryFilterOptions<TEntity> filterOptions, GenericSortOptions<TEntity> sortOptions); // probably will transition towards this one
        public Task<TEntity> SearchFirstAsync(Expression<Func<TEntity, bool>> predicate);
        public IQueryable<TEntity> GetAll();
        public Task<TEntity> GetByIdAsync(TKey id);
        // CRUD
        public Task CreateAsync(TEntity entity, params object[] extraParams);
        public Task UpdateAsync(TEntity entity);
        public Task RemoveByIdAsync(TKey guid);
        public Task<int> SaveChangesAsync();
        public void Dispose();
    }
}
