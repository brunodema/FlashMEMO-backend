using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IBaseRepository<TEntity, DatabaseContext> where TEntity : class
        where DatabaseContext : DbContext
    {
        public Task<IEnumerable<TEntity>> SearchAndOrderAsync<TKey>(Expression<Func<TEntity, bool>> predicate, SortType sortType, Expression<Func<TEntity, TKey>> columnToSort, int numRecords);
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
    public enum SortType
    {
        None,
        Ascending,
        Descending
    }
    public class SortOptions<TEntity, IKey>
    {
        public SortType SortType { get; set; } = SortType.None;
        public Expression<Func<TEntity, IKey>> ColumnToSort { get; set; } = null;
    }
}
