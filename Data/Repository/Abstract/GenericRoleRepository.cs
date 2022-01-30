using Data.Messages;
using Data.Repository.Interfaces;
using Data.Tools.Implementation;
using Data.Tools.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository.Implementation
{
    public class GenericRoleRepository<TEntity, TKey, DatabaseContext> : IRepository<TEntity, TKey, IdentityResult>
        where TEntity : IdentityRole<string>, IDatabaseItem<TKey>
        where DatabaseContext : DbContext
    {
        protected readonly RoleManager<TEntity> _roleManager;
        protected readonly DatabaseContext _context;

        public GenericRoleRepository(DatabaseContext context, RoleManager<TEntity> roleManager)
        {
            _roleManager = roleManager;
            _context = context;
        }
        public virtual IEnumerable<TEntity> SearchAndOrderAsync(Expression<Func<TEntity, bool>> predicate, GenericSortOptions<TEntity> sortOptions, int numRecords)
        {
            return sortOptions.GetSortedResults(_roleManager.Roles.AsNoTracking().Where(predicate)).Take(numRecords);
        }
        public virtual async Task<IEnumerable<TEntity>> SearchAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _roleManager.Roles.AsNoTracking().Where(predicate).ToListAsync();
        }
        public virtual async Task<TEntity> SearchFirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _roleManager.Roles.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            return _roleManager.Roles.AsQueryable();
        }
        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }
        // CRUD
        public virtual async Task<IdentityResult> CreateAsync(TEntity entity)
        {
            var result = await _roleManager.CreateAsync(entity);
            await SaveChangesAsync();
            return result;
        }
        public virtual async Task<IdentityResult> UpdateAsync(TEntity entity)
        {
            var result = await _roleManager.UpdateAsync(entity);
            await SaveChangesAsync();
            return result;
        }
        public virtual async Task RemoveByIdAsync(TKey guid)
        {
            var entity = await _roleManager.FindByIdAsync(guid.ToString());
            if (entity == null)
            {
                throw new Exception(RepositoryExceptionMessages.NullObjectInvalidID);
            }
            await _roleManager.DeleteAsync(entity);
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
        public IEnumerable<TEntity> SearchAndOrder(IQueryFilterOptions<TEntity> filterOptions, GenericSortOptions<TEntity> sortOptions)
        {
            return sortOptions.GetSortedResults(filterOptions.GetFilteredResults(GetAll()).AsQueryable());
        }
    }
}
