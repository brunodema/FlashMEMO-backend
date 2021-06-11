using Data.Context;
using Data.Models;
using Data.Tools;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class ApplicationUserRepository : BaseRepository<ApplicationUser, string, FlashMEMOContext>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUserRepository(FlashMEMOContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
        }
        public override async Task<IEnumerable<ApplicationUser>> SearchAndOrderAsync<ColumnType>(Expression<Func<ApplicationUser, bool>> predicate, SortOptions<ApplicationUser, ColumnType> sortOptions, int numRecords)
        {
            if (sortOptions != null)
            {
                if (sortOptions.SortType == SortType.Ascending)
                {
                    return await _userManager.Users.AsNoTracking().Where(predicate).OrderBy(sortOptions.ColumnToSort).Take(numRecords).ToListAsync();
                }
                return await _userManager.Users.AsNoTracking().Where(predicate).OrderByDescending(sortOptions.ColumnToSort).Take(numRecords).ToListAsync();
            }
            return await _userManager.Users.AsNoTracking().Where(predicate).Take(numRecords).ToListAsync();
        }
        public override async Task<IEnumerable<ApplicationUser>> SearchAllAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _userManager.Users.AsNoTracking().Where(predicate).ToListAsync();
        }
        public override async Task<ApplicationUser> SearchFirstAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        public override async Task<ICollection<ApplicationUser>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        public override async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }
        // CRUD
        // Note: this is the intended method when creating a user, since this 'override' uses the 'UserManager' instead of context.
        public override async Task<IdentityResult> CreateAsync(ApplicationUser entity, object[] extraParams)
        {
            return await _userManager.CreateAsync(entity, (string)extraParams[0]);
        }
        public override async Task<IdentityResult> UpdateAsync(ApplicationUser entity)
        {
            return await _userManager.UpdateAsync(entity);
        }
        public async Task<IdentityResult> AddUserToRoleAsync(ApplicationUser entity, ApplicationRole role)
        {
            return await _userManager.AddToRoleAsync(entity, role.Name);
        }
        public async Task<IdentityResult> RemoveUserFromRoleAsync(ApplicationUser entity, ApplicationRole role)
        {
            return await _userManager.RemoveFromRoleAsync(entity, role.Name);
        }
        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string cleanPassword)
        {
            return await _userManager.CheckPasswordAsync(user, cleanPassword);
        }
        public override void Dispose()
        {
            _userManager?.Dispose();
            base.Dispose();
        }
    }
}
