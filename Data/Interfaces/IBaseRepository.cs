using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IBaseRepository<T>
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(Guid id);
    }

    public abstract class BaseRepository<TEntity, DatabaseContext> : IBaseRepository<TEntity>
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
        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbset.ToListAsync();
        }
        public async Task<TEntity> GetById(Guid id)
        {
            return await _dbset.FindAsync(id);
        }
        // CRUD
        public virtual async Task Create(TEntity entity)
        {
            _dbset.Add(entity);
            await SaveChanges();
        }
        public virtual async Task Update(TEntity entity)
        {
            _dbset.Update(entity);
            await SaveChanges();
        }
        public virtual async Task Remove(TEntity entity)
        {
            _dbset.Remove(entity);
            await SaveChanges();
        }
        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
