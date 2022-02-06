using Data.Tools.Sorting;
using Data.Tools.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    public interface IRepository<TEntity, TKey, TContextResult> 
        where TEntity : class, IDatabaseItem<TKey>
    {
        /// <summary>
        /// This method is now deprecated. Use <seealso cref="SearchAndOrder(IQueryFilterOptions{TEntity}, GenericSortOptions{TEntity}) instead. This will be removed once all dependent function/classes are refactored."/>
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="sortOptions"></param>
        /// <param name="numRecords"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> SearchAndOrder(Expression<Func<TEntity, bool>> predicate, GenericSortOptions<TEntity> sortOptions, int numRecords);
        public IEnumerable<TEntity> SearchAndOrder(IQueryFilterOptions<TEntity> filterOptions, GenericSortOptions<TEntity> sortOptions); // probably will transition towards this one
        public IQueryable<TEntity> GetAll();
        public Task<TEntity> GetByIdAsync(TKey id);
        // CRUD
        public Task<TContextResult> CreateAsync(TEntity entity);
        public Task<TContextResult> UpdateAsync(TEntity entity);
        public Task RemoveByIdAsync(TKey guid);
        public Task<int> SaveChangesAsync();
        public void Dispose();
    }
}
