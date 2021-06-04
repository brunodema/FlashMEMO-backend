﻿using Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Tools;

namespace Data.Repository.Interfaces
{
    public interface IAuthRepository
    {
        #region USER
        public Task<IEnumerable<ApplicationUser>> SearchAndOrderAsync<TKey>(Expression<Func<ApplicationUser, bool>> predicate, SortOptions<ApplicationUser, TKey> sortOptions, int numRecords);
        public Task<ApplicationUser> SearchFirstAsync(Expression<Func<ApplicationUser, bool>> predicate);
        public Task<ICollection<ApplicationUser>> GetAllUsersAsync();
        public Task<ApplicationUser> GetUserByIdAsync(Guid id);
        public Task<bool> CheckPasswordAsync(ApplicationUser user, string cleanPassword);
        // CRUD
        public Task<IdentityResult> CreateAsync(ApplicationUser entity, string cleanPassword);
        public Task<IdentityResult> UpdateAsync(ApplicationUser entity);
        public Task<IdentityResult> RemoveAsync(ApplicationUser entity);
        #endregion
        #region ROLE
        public Task<IEnumerable<ApplicationRole>> SearchAndOrderAsync<TKey>(Expression<Func<ApplicationRole, bool>> predicate, SortOptions<ApplicationRole, TKey> sortOptions, int numRecords);
        public Task<ApplicationRole> SearchFirstAsync(Expression<Func<ApplicationRole, bool>> predicate);
        public Task<ICollection<ApplicationRole>> GetAllRolesAsync();
        public Task<ApplicationRole> GetRoleByIdAsync(Guid id);
        // CRUD
        public Task<IdentityResult> CreateAsync(ApplicationRole entity);
        public Task<IdentityResult> UpdateAsync(ApplicationRole entity);
        public Task<IdentityResult> RemoveAsync(ApplicationRole entity);
        #endregion
        public Task AdduserToRoleAsync(ApplicationUser user, ApplicationRole role);
        public Task RemoveUserFromRoleAsync(ApplicationUser user, ApplicationRole role);
        public Task<IEnumerable<ApplicationRole>> GetUserRolesAsync(ApplicationUser user);
        public void Dispose();
    }
}
