using Data.Messages;
using Data.Repository.Interfaces;
using Data.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository
{
    public abstract class BaseRepository<TEntity, TKey, DatabaseContext> : IBaseRepository<TEntity, TKey>
        where TEntity : class, IDatabaseItem<TKey>
        where DatabaseContext : DbContext
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<TEntity> _dbset;

        protected BaseRepository(DatabaseContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }
        public virtual async Task<IEnumerable<TEntity>> SearchAndOrderAsync(Expression<Func<TEntity, bool>> predicate, GenericSortOptions<TEntity> sortOptions, int numRecords)
        {
            if (sortOptions != null)
            {
                if (sortOptions.SortType == SortType.Ascending)
                {
                    return await _dbset.AsNoTracking().Where(predicate).OrderBy(sortOptions.ColumnToSort).Take(numRecords).ToListAsync();
                }
                return await _dbset.AsNoTracking().Where(predicate).OrderByDescending(sortOptions.ColumnToSort).Take(numRecords).ToListAsync();
            }
            return await _dbset.AsNoTracking().Where(predicate).Take(numRecords).ToListAsync();
        }
        public virtual async Task<IEnumerable<TEntity>> SearchAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbset.AsNoTracking().Where(predicate).ToListAsync();
        }
        public virtual async Task<TEntity> SearchFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbset.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        public virtual async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }
        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _dbset.FindAsync(id);
        }
        // CRUD
        public virtual async Task CreateAsync(TEntity entity, object[] extraParams = null)
        {
            _dbset.Add(entity);
            await SaveChangesAsync();
        }
        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dbset.Update(entity);
            await SaveChangesAsync();
        }
        public virtual async Task RemoveByIdAsync(TKey guid)
        {
            var entity = await _dbset.FindAsync(guid);
            if (entity == null)
            {
                throw new Exception(RepositoryExceptionMessages.NullObjectInvalidID);
            }
            _dbset.Remove(entity);
            await SaveChangesAsync();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public virtual void Dispose()
        {
            _context?.Dispose();
        }
    }
}
