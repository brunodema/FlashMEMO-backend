using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Data.Models;

namespace Data.Interfaces
{
    public interface IApplicationUserRepository
    {
        public Task<IEnumerable<ApplicationUser>> SearchAndOrderUserAsync<TKey>(Expression<Func<ApplicationUser, bool>> predicate, SortType sortType, Expression<Func<ApplicationUser, TKey>> columnToSort, int numRecords);
        public Task<IEnumerable<ApplicationUser>> SearchAllUserAsync(Expression<Func<ApplicationUser, bool>> predicate);
        public Task<ApplicationUser> SearchFirstAsync(Expression<Func<ApplicationUser, bool>> predicate);
        public Task<ICollection<ApplicationUser>> GetAllUserAsync();
        public Task<ApplicationUser> GetUserByIdAsync(Guid id);
        // CRUD
        public Task<IdentityResult> CreateUserAsync(ApplicationUser entity, string encryptedPassword);
        public Task<IdentityResult> UpdateUserAsync(ApplicationUser entity);
        public Task<IdentityResult> RemoveUserAsync(ApplicationUser entity);
        public void Dispose();
    }
}

