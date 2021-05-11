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
        Ascending,
        Descending
    }

    public abstract class BaseRepository<TEntity, DatabaseContext> : IBaseRepository<TEntity , DatabaseContext>
        where TEntity : class
        where DatabaseContext : DbContext
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<TEntity> _dbset;

        protected BaseRepository(DatabaseContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> SearchAndOrderAsync<TKey>(Expression<Func<TEntity, bool>> predicate, SortType sortType, Expression<Func<TEntity, TKey>> columnToSort, int numRecords)
        {
            if (sortType == SortType.Ascending)
            {
                return await _dbset.AsNoTracking().Where(predicate).OrderBy(columnToSort).Take(numRecords).ToListAsync();
            }
            return await _dbset.AsNoTracking().Where(predicate).OrderByDescending(columnToSort).Take(numRecords).ToListAsync();

        }
        public async Task<IEnumerable<TEntity>> SearchAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbset.AsNoTracking().Where(predicate).ToListAsync();
        }
        public async Task<TEntity> SearchFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbset.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        public async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await _dbset. ToListAsync();
        }
        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbset.FindAsync(id);
        }
        // CRUD
        public virtual async Task CreateAsync(TEntity entity)
        {
            _dbset.Add(entity);
            await SaveChangesAsync();
        }
        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dbset.Update(entity);
            await SaveChangesAsync();
        }
        public virtual async Task RemoveAsync(TEntity entity)
        {
            _dbset.Remove(entity);
            await SaveChangesAsync();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
