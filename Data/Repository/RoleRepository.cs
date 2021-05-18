using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class RoleRepository : BaseRepository<ApplicationRole, FlashMEMOContext>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RoleRepository(FlashMEMOContext context, RoleManager<ApplicationRole> roleManager) : base(context)
        {
            _roleManager = roleManager;
        }
        public override async Task<IEnumerable<ApplicationRole>> SearchAndOrderAsync<TKey>(Expression<Func<ApplicationRole, bool>> predicate, SortType sortType, Expression<Func<ApplicationRole, TKey>> columnToSort, int numRecords)
        {
            if (sortType == SortType.Ascending)
            {
                return await _roleManager.Roles.AsNoTracking().Where(predicate).OrderBy(columnToSort).Take(numRecords).ToListAsync();
            }
            return await _roleManager.Roles.AsNoTracking().Where(predicate).OrderByDescending(columnToSort).Take(numRecords).ToListAsync();

        }
        public override async Task<IEnumerable<ApplicationRole>> SearchAllAsync(Expression<Func<ApplicationRole, bool>> predicate)
        {
            return await _roleManager.Roles.AsNoTracking().Where(predicate).ToListAsync();
        }
        public override async Task<ApplicationRole> SearchFirstAsync(Expression<Func<ApplicationRole, bool>> predicate)
        {
            return await _roleManager.Roles.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        public override async Task<ICollection<ApplicationRole>> GetAllAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }
        public override async Task<ApplicationRole> GetByIdAsync(Guid id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }
        // CRUD
        // Note: this is the intended method when creating a user, since this 'override' uses the 'UserManager' instead of context.
        public override async Task<IdentityResult> CreateAsync(ApplicationRole entity)
        {
            return await _roleManager.CreateAsync(entity);
        }
        public override async Task<IdentityResult> UpdateAsync(ApplicationRole entity)
        {
            return await _roleManager.UpdateAsync(entity);
        }
        public override async Task<IdentityResult> RemoveAsync(ApplicationRole entity)
        {
            return await _roleManager.DeleteAsync(entity);
        }
        public override void Dispose()
        {
            _roleManager?.Dispose();
            base.Dispose();
        }
    }
}
