using Data.Tools.Sorting;
using Data.Tools.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository.Interfaces
{
    /// <summary>
    /// Interface that defines the standard behavior of classes that handle CRUD operations directly with the app's DB context (FlashMEMOContext).
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TIdentityResult">This class is a bit weird. Since in the case of user/role some operations return IdentityResult, this parameter exist to preserve that return type in other interfaces. In usual cases, it will be either string or GUID (same as TKey).</typeparam>
    public interface IRepository<TEntity, TKey, TIdentityResult> 
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
        public Task<TIdentityResult> CreateAsync(TEntity entity);
        public Task<TIdentityResult> UpdateAsync(TEntity entity);
        public Task<TIdentityResult> RemoveByIdAsync(TKey guid);
        public Task<int> SaveChangesAsync();
        public void Dispose();
    }
}
