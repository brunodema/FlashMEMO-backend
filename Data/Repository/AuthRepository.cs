using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationUserRepository _applicationUserRepository;
        private readonly RoleRepository _roleRepository;
        private readonly DbContext _context;

        public AuthRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationUserRepository applicationUserRepository, RoleRepository roleRepository, DbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _applicationUserRepository = applicationUserRepository;
            _roleRepository = roleRepository;
            _context = context;
        }

        public async Task AdduserToRule(ApplicationUser user, ApplicationRole role)
        {
            await _userManager.AddToRoleAsync(user, role.Name);
        }

        public async Task CreateAsync(ApplicationUser entity, string cleanPassword)
        {
            await _userManager.CreateAsync(entity, cleanPassword);
        }

        public async Task CreateAsync(ApplicationRole entity)
        {
            await _roleManager.CreateAsync(entity);
        }

        public void Dispose()
        {
            _userManager?.Dispose();
            _roleManager?.Dispose(); 
            _applicationUserRepository?.Dispose();
            _roleRepository?.Dispose();
            _context?.Dispose();
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

        public Task RemoveAsync(ApplicationUser entity)
        {
            return _applicationUserRepository.RemoveAsync(entity);
        }

        public Task RemoveAsync(ApplicationRole entity)
        {
            return _roleRepository.RemoveAsync(entity);
        }

        public async Task RemoveUserFromRule(ApplicationUser user, ApplicationRole role)
        {
            await _userManager.RemoveFromRoleAsync(user, role.Name);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> SearchAllAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _applicationUserRepository.SearchAllAsync(predicate);
        }

        public async Task<IEnumerable<ApplicationRole>> SearchAllAsync(Expression<Func<ApplicationRole, bool>> predicate)
        {
            return await _roleRepository.SearchAllAsync(predicate);
        }

        public async Task<IEnumerable<ApplicationUser>> SearchAndOrderAsync<TKey>(Expression<Func<ApplicationUser, bool>> predicate, SortType sortType, Expression<Func<ApplicationUser, TKey>> columnToSort, int numRecords)
        {
            return await _applicationUserRepository.SearchAndOrderAsync(predicate, sortType, columnToSort, numRecords);
        }

        public async Task<IEnumerable<ApplicationRole>> SearchAndOrderAsync<TKey>(Expression<Func<ApplicationRole, bool>> predicate, SortType sortType, Expression<Func<ApplicationRole, TKey>> columnToSort, int numRecords)
        {
            return await _roleRepository.SearchAndOrderAsync(predicate, sortType, columnToSort, numRecords);
        }

        public async Task<ApplicationUser> SearchFirstAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _applicationUserRepository.SearchFirstAsync(predicate);
        }

        public async Task<ApplicationRole> SearchFirstAsync(Expression<Func<ApplicationRole, bool>> predicate)
        {
            return await _roleRepository.SearchFirstAsync(predicate);
        }

        public async Task UpdateAsync(ApplicationUser entity)
        {
            await _applicationUserRepository.UpdateAsync(entity);
        }

        public async Task UpdateAsync(ApplicationRole entity)
        {
            await _roleRepository.UpdateAsync(entity);
        }
    }
}
