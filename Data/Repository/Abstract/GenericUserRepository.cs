using Data.Messages;
using Data.Repository.Interfaces;
using Data.Tools.Sorting;
using Data.Tools.Filtering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository.Implementation
{
    public class GenericUserRepository<TEntity, TKey, DatabaseContext> : IRepository<TEntity, TKey, IdentityResult>
        where TEntity : IdentityUser<string>, IDatabaseItem<TKey>
        where DatabaseContext : DbContext
    {
        protected readonly UserManager<TEntity> _userManager;
        protected readonly DatabaseContext _context;

        public GenericUserRepository(DatabaseContext context, UserManager<TEntity> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public virtual IEnumerable<TEntity> SearchAndOrder(Expression<Func<TEntity, bool>> predicate, GenericSortOptions<TEntity> sortOptions, int numRecords)
        {
            return sortOptions.GetSortedResults(_userManager.Users.Where(predicate)).Take(numRecords);
        }
        public virtual async Task<IEnumerable<TEntity>> SearchAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _userManager.Users.Where(predicate).ToListAsync();
        }
        public virtual async Task<TEntity> SearchFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _userManager.Users.FirstOrDefaultAsync(predicate);
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            return _userManager.Users.AsQueryable();
        }
        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }
        // CRUD
        public virtual async Task<IdentityResult> CreateAsync(TEntity entity)
        {
            var result = await _userManager.CreateAsync(entity);
            await SaveChangesAsync();
            return result;
        }
        public virtual async Task<IdentityResult> UpdateAsync(TEntity entity)
        {
            var result = await _userManager.UpdateAsync(entity);
            await SaveChangesAsync();
            return result;
        }
        public virtual async Task<IdentityResult> RemoveByIdAsync(TKey guid)
        {
            var entity = await _userManager.FindByIdAsync(guid.ToString());
            if (entity == null)
            {
                throw new Exception(RepositoryExceptionMessages.NullObjectInvalidID);
            }
            var ret = await _userManager.DeleteAsync(entity);
            await SaveChangesAsync();

            return ret;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public virtual void Dispose()
        {
            _context?.Dispose();
        }
        public IEnumerable<TEntity> SearchAndOrder(IQueryFilterOptions<TEntity> filterOptions, GenericSortOptions<TEntity> sortOptions)
        {
            return sortOptions.GetSortedResults(filterOptions.GetFilteredResults(GetAll()).AsQueryable());
        }

        // Custom functions
        public async Task SetInitialPassword(TKey id, string password)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            await _userManager.AddPasswordAsync(user, password);
        }
        public async Task<bool> CheckPasswordAsync(TKey id, string password)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}
