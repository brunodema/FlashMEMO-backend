using Data.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    public interface IBaseRepository<TEntity>
        where TEntity : class
    {
        public Task<IEnumerable<TEntity>> SearchAndOrderAsync<TKey>(Expression<Func<TEntity, bool>> predicate, SortOptions<TEntity, TKey> sortOptions, int numRecords);
        public Task<IEnumerable<TEntity>> SearchAllAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<TEntity> SearchFirstAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<ICollection<TEntity>> GetAllAsync();
        public Task<TEntity> GetByIdAsync(Guid id);
        // CRUD
        public Task CreateAsync(TEntity entity);
        public Task UpdateAsync(TEntity entity);
        public Task RemoveAsync(TEntity entity);
        public Task<int> SaveChangesAsync();
        public void Dispose();
    }
}
