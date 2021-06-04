using Data.Models;
using Data.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Tools;

namespace Data.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationUserRepository _applicationUserRepository;
        private readonly RoleRepository _roleRepository;

        public AuthRepository(ApplicationUserRepository applicationUserRepository, RoleRepository roleRepository)
        {
            _applicationUserRepository = applicationUserRepository;
            _roleRepository = roleRepository;
        }

        public async Task AdduserToRuleAsync(ApplicationUser user, ApplicationRole role)
        {
            await _applicationUserRepository.AddUserToRoleAsync(user, role);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string cleanPassword)
        {
            return await _applicationUserRepository.CheckPasswordAsync(user, cleanPassword);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser entity, string cleanPassword)
        {
            return await _applicationUserRepository.CreateUserAsync(entity, cleanPassword);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationRole entity)
        {
            return await  _roleRepository.CreateAsync(entity);
        }

        public void Dispose()
        {
            _applicationUserRepository?.Dispose();
            _roleRepository?.Dispose();
        }

        public async Task<ICollection<ApplicationRole>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllAsync();
        }

        public async Task<ICollection<ApplicationUser>> GetAllUsersAsync()
        {
            return await _applicationUserRepository.GetAllAsync();
        }

        public Task<ApplicationRole> GetRoleByIdAsync(Guid id)
        {
            return _roleRepository.GetByIdAsync(id);
        }

        public Task<ApplicationUser> GetUserByIdAsync(Guid id)
        {
            return _applicationUserRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ApplicationRole>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _roleRepository.SearchAllAsync(role => role.UserRoles.Any(r => r.User == user)); // probably will break
        }

        public Task<IdentityResult> RemoveAsync(ApplicationUser entity)
        {
            return _applicationUserRepository.RemoveAsync(entity);
        }

        public Task<IdentityResult> RemoveAsync(ApplicationRole entity)
        {
            return _roleRepository.RemoveAsync(entity);
        }

        public async Task RemoveUserFromRoleAsync(ApplicationUser user, ApplicationRole role)
        {
            await _applicationUserRepository.RemoveUserFromRoleAsync(user, role);
        }

        public async Task<IEnumerable<ApplicationUser>> SearchAndOrderAsync<TKey>(Expression<Func<ApplicationUser, bool>> predicate, SortOptions<ApplicationUser, TKey> sortOptions, int numRecords)
        {
            return await _applicationUserRepository.SearchAndOrderAsync<TKey>(predicate, sortOptions, numRecords);
        }

        public async Task<IEnumerable<ApplicationRole>> SearchAndOrderAsync<TKey>(Expression<Func<ApplicationRole, bool>> predicate, SortOptions<ApplicationRole, TKey> sortOptions, int numRecords)
        {
            return await _roleRepository.SearchAndOrderAsync(predicate, sortOptions, numRecords);
        }

        public async Task<ApplicationUser> SearchFirstAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _applicationUserRepository.SearchFirstAsync(predicate);
        }

        public async Task<ApplicationRole> SearchFirstAsync(Expression<Func<ApplicationRole, bool>> predicate)
        {
            return await _roleRepository.SearchFirstAsync(predicate);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser entity)
        {
            return await _applicationUserRepository.UpdateAsync(entity);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationRole entity)
        {
            return await _roleRepository.UpdateAsync(entity);
        }
    }
}
