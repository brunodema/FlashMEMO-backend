using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IAuthRepository
    {
        #region USER
        public Task<IEnumerable<ApplicationUser>> SearchAndOrderAsync<TKey>(Expression<Func<ApplicationUser, bool>> predicate, SortType sortType, Expression<Func<ApplicationUser, TKey>> columnToSort, int numRecords);
        public Task<IEnumerable<ApplicationUser>> SearchAllAsync(Expression<Func<ApplicationUser, bool>> predicate);
        public Task<ApplicationUser> SearchFirstAsync(Expression<Func<ApplicationUser, bool>> predicate);
        public Task<ICollection<ApplicationUser>> GetAllUsersAsync();
        public Task<ApplicationUser> GetUserByIdAsync(Guid id);
        // CRUD
        public Task CreateAsync(ApplicationUser entity, string cleanPassword);
        public Task UpdateAsync(ApplicationUser entity);
        public Task RemoveAsync(ApplicationUser entity);
        #endregion
        #region ROLE
        public Task<IEnumerable<ApplicationRole>> SearchAndOrderAsync<TKey>(Expression<Func<ApplicationRole, bool>> predicate, SortType sortType, Expression<Func<ApplicationRole, TKey>> columnToSort, int numRecords);
        public Task<IEnumerable<ApplicationRole>> SearchAllAsync(Expression<Func<ApplicationRole, bool>> predicate);
        public Task<ApplicationRole> SearchFirstAsync(Expression<Func<ApplicationRole, bool>> predicate);
        public Task<ICollection<ApplicationRole>> GetAllRolesAsync();
        public Task<ApplicationRole> GetRoleByIdAsync(Guid id);
        // CRUD
        public Task CreateAsync(ApplicationRole entity);
        public Task UpdateAsync(ApplicationRole entity);
        public Task RemoveAsync(ApplicationRole entity);
        #endregion
        public Task AdduserToRule(ApplicationUser user, ApplicationRole role);
        public Task RemoveUserFromRule(ApplicationUser user, ApplicationRole role);
        public Task<int> SaveChangesAsync();
        public void Dispose();
    }
}
