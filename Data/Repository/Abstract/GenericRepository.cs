using Data.Repository.Interfaces;
using Data.Tools.Sorting;
using Data.Tools.Filtering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository.Abstract
{
    public abstract class GenericRepository<TEntity, TKey, DatabaseContext> : IRepository<TEntity, TKey, TKey>
        where TEntity : class, IDatabaseItem<TKey>
        where DatabaseContext : DbContext
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<TEntity> _dbset;

        protected GenericRepository(DatabaseContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }
        /// <summary>
        /// After applying filtering according to the provided <paramref name="predicate"/>, and ordering the results by the especified <paramref name="sortOptions"/>, returns a collection of at most <paramref name="numRecords"/>. If <paramref name="numRecords"/> is a negative number, it returns the full collection instead.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="sortOptions"></param>
        /// <param name="numRecords"> Maximum numnber of records to be returned. If less than 0, returns all records instead.</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> SearchAndOrder(Expression<Func<TEntity, bool>> predicate, GenericSortOptions<TEntity> sortOptions = null, int numRecords = 10)
        {
            if (numRecords < 0)
            {
                return sortOptions?.GetSortedResults(_dbset.Where(predicate)) ?? _dbset.Where(predicate);
            }
            return sortOptions?.GetSortedResults(_dbset.Where(predicate)).Take(numRecords) ?? _dbset.Where(predicate).Take(numRecords);
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbset.AsQueryable();
        }
        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _dbset.FindAsync(id);
        }
        // CRUD
        public virtual async Task<TKey> CreateAsync(TEntity entity)
        {
            _dbset.Add(entity);
            await SaveChangesAsync();
            return entity.DbId;
        }
        public virtual async Task<TKey> UpdateAsync(TEntity entity)
        {
            //var objFromDB = _dbset.ToList().FirstOrDefault(o => o.DbId.Equals(entity.DbId));
            //objFromDB = entity;

            //_dbset.Update(objFromDB);
            //await SaveChangesAsync();
            //return entity.DbId;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity.DbId;
        }
        public virtual async Task<TKey> RemoveByIdAsync(TKey guid)
        {
            var entity = await _dbset.FindAsync(guid);
            if (entity == null) return default(TKey);

            _dbset.Remove(entity);
            var ret = await SaveChangesAsync();

            return ret > 0 ? entity.DbId : default(TKey);
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public virtual void Dispose()
        {
            _context?.Dispose();
        }
        public virtual IEnumerable<TEntity> SearchAndOrder(IQueryFilterOptions<TEntity> filterOptions, GenericSortOptions<TEntity> sortOptions)
        {
            return sortOptions.GetSortedResults(filterOptions.GetFilteredResults(GetAll()).AsQueryable());
        }
    }
}
