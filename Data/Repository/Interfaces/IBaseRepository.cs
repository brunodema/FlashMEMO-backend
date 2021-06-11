using Data.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    public interface IBaseRepository<TEntity, TKey>
    {
        public Task<IEnumerable<TEntity>> SearchAndOrderAsync<ColumnType>(Expression<Func<TEntity, bool>> predicate, SortOptions<TEntity, ColumnType> sortOptions, int numRecords);
        public Task<IEnumerable<TEntity>> SearchAllAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<TEntity> SearchFirstAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<ICollection<TEntity>> GetAllAsync();
        public Task<TEntity> GetByIdAsync(TKey id);
        // CRUD
        public Task CreateAsync(TEntity entity, params object[] extraParams);
        public Task UpdateAsync(TEntity entity);
        public Task RemoveByIdAsync(TKey guid);
        public Task<int> SaveChangesAsync();
        public void Dispose();
    }
}
