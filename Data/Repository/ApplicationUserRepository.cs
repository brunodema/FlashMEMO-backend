using Data;
using Data.Interfaces;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class ApplicationUserRepository : BaseRepository<ApplicationUser, FlashMEMOContext>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUserRepository(FlashMEMOContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
        }
        public override async Task<IEnumerable<ApplicationUser>> SearchAndOrderAsync<TKey>(Expression<Func<ApplicationUser, bool>> predicate, SortType sortType, Expression<Func<ApplicationUser, TKey>> columnToSort, int numRecords)
        {
            if (sortType == SortType.Ascending)
            {
                return await _userManager.Users.AsNoTracking().Where(predicate).OrderBy(columnToSort).Take(numRecords).ToListAsync();
            }
            return await _userManager.Users.AsNoTracking().Where(predicate).OrderByDescending(columnToSort).Take(numRecords).ToListAsync();

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
        public override async Task<ApplicationUser> GetByIdAsync(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }
        // CRUD
        // Note: this is the intended method when creating a user, since this 'override' uses the 'UserManager' instead of context.
        public async Task<IdentityResult> CreateUserAsync(ApplicationUser entity, string encryptedPassword)
        {
            return await _userManager.CreateAsync(entity, encryptedPassword);
        }
        public override async Task<IdentityResult> UpdateAsync(ApplicationUser entity)
        {
            return await _userManager.UpdateAsync(entity);
        }
        public override async Task<IdentityResult> RemoveAsync(ApplicationUser entity)
        {
            return await _userManager.DeleteAsync(entity);
        }
        public async Task<IdentityResult> AddUserToRoleAsync(ApplicationUser entity, ApplicationRole role)
        {
            return await _userManager.AddToRoleAsync(entity, role.Name);
        }
        public async Task<IdentityResult> RemoveUserFromRoleAsync(ApplicationUser entity, ApplicationRole role)
        {
            return await _userManager.RemoveFromRoleAsync(entity, role.Name);
        }
        public override void Dispose()
        {
            _userManager?.Dispose();
            base.Dispose();
        }
    }
}
