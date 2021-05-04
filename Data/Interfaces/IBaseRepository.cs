﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IBaseRepository<T>
    {
        public Task<ICollection<T>> GetAllAsync();
        public Task<T> GetByIdAsync(Guid id);
        public Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        public Task<T> FindFirstAsync(Expression<Func<T, bool>> predicate);

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
        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbset.AsNoTracking().Where(predicate).ToListAsync();
        }
        public async Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbset.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        public async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
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
