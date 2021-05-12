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
    public class AuthRepository : IAuthRepository
    {
        protected readonly UserManager<ApplicationUser> _userManager;

        public AuthRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IEnumerable<ApplicationUser>> SearchAndOrderUserAsync<TKey>(Expression<Func<ApplicationUser, bool>> predicate, SortType sortType, Expression<Func<ApplicationUser, TKey>> columnToSort, int numRecords)
        {
            if (sortType == SortType.Ascending)
            {
                return await _userManager.Users.AsNoTracking().Where(predicate).OrderBy(columnToSort).Take(numRecords).ToListAsync();
            }
            return await _userManager.Users.AsNoTracking().Where(predicate).OrderByDescending(columnToSort).Take(numRecords).ToListAsync();

        }
        public async Task<IEnumerable<ApplicationUser>> SearchAllUserAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _userManager.Users.AsNoTracking().Where(predicate).ToListAsync();
        }
        public async Task<ApplicationUser> SearchFirstAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        public async Task<ICollection<ApplicationUser>> GetAllUserAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        public async Task<ApplicationUser> GetUserByIdAsync(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }
        // CRUD
        public virtual async Task<IdentityResult> CreateUserAsync(ApplicationUser entity, string encryptedPassword)
        {
            return await _userManager.CreateAsync(entity, encryptedPassword);
        }
        public virtual async Task<IdentityResult> UpdateUserAsync(ApplicationUser entity)
        {
            return await _userManager.UpdateAsync(entity);
        }
        public virtual async Task<IdentityResult> RemoveUserAsync(ApplicationUser entity)
        {
            return await _userManager.DeleteAsync(entity);
        }
        public void Dispose()
        {
            _userManager?.Dispose();
        }
    }
}