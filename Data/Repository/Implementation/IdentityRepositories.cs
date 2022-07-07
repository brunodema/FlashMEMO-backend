using Data.Context;
using Data.Models.Implementation;
using Data.Repository.Abstract;
using Data.Tools.Sorting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository.Implementation
{
    public interface IUserRepository<TUserType>
    {
        Task SetInitialPasswordAsync(TUserType user, string newPassword);
        Task<TUserType> GetByEmailAsync(string email);
        Task<TUserType> GetByUserNameAsync(string username);
        Task<bool> CheckPasswordAsync(TUserType user, string initialPassword);
        Task<string> GeneratePasswordResetToken(User user);
        Task UpdatePasswordAsync(User user, string resetToken, string newPassword);
    }

    public class UserRepository : GenericRepository<User, string, FlashMEMOContext>, IUserRepository<User>
    {
        protected UserManager<User> _userManager;

        public UserRepository(FlashMEMOContext context, UserManager<User> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public override IEnumerable<User> SearchAndOrder(Expression<Func<User, bool>> predicate, GenericSortOptions<User> sortOptions = null, int numRecords = 10)
        {
            if (numRecords < 0)
            {
                return sortOptions?.GetSortedResults(_userManager.Users.Where(predicate)) ?? _userManager.Users.Where(predicate);
            }
            return sortOptions?.GetSortedResults(_userManager.Users.Where(predicate)).Take(numRecords) ?? _userManager.Users.Where(predicate).Take(numRecords);
        }

        public override IQueryable<User> GetAll()
        {
            return _userManager.Users.AsQueryable();
        }

        public override async Task<User> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        // CRUD
        public override async Task<string> CreateAsync(User entity)
        {
            await _userManager.CreateAsync(entity);
            await SaveChangesAsync();
            return entity.DbId;
        }

        public override async Task<string> UpdateAsync(User entity)
        {
            //var objFromDB = _userManager.Users.FirstOrDefault(u => u.Id == entity.Id);
            //objFromDB = entity;

            //await _userManager.UpdateAsync(objFromDB);
            //await SaveChangesAsync();
            //return entity.DbId;

            _context.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();
            return entity.DbId;
        }

        public override async Task<string> RemoveByIdAsync(string guid)
        {
            var entity = await GetByIdAsync(guid);
            if (entity == null)
            {
                return default(string);
            }
            var ret = await _userManager.DeleteAsync(entity);
            await SaveChangesAsync();

            return ret.Succeeded ? entity.DbId : default(string);
        }

        public override void Dispose()
        {
            _context?.Dispose();
            _userManager.Dispose();
        }

        // inherited from IUserRepository

        public async Task SetInitialPasswordAsync(User user, string newPassword)
        {
            await _userManager.AddPasswordAsync(user, newPassword);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetByUserNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public Task<string> GeneratePasswordResetToken(User user)
        {
            return _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task UpdatePasswordAsync(User user, string resetToken, string newPassword)
        {
            await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            await SaveChangesAsync();
        }
    }

    public interface IRoleRepository<TRoleType>
    {
        public Task<TRoleType> GetByRoleNameAsync(string roleName);
    }

    public class RoleRepository : GenericRepository<Role, string, FlashMEMOContext>, IRoleRepository<Role>
    {
        protected RoleManager<Role> _roleManager;

        public RoleRepository(FlashMEMOContext context, RoleManager<Role> roleManager) : base(context)
        {
            _roleManager = roleManager;
        }

        public override IEnumerable<Role> SearchAndOrder(Expression<Func<Role, bool>> predicate, GenericSortOptions<Role> sortOptions = null, int numRecords = 10)
        {
            if (numRecords < 0)
            {
                return sortOptions?.GetSortedResults(_roleManager.Roles.Where(predicate)) ?? _roleManager.Roles.Where(predicate);
            }
            return sortOptions?.GetSortedResults(_roleManager.Roles.Where(predicate)).Take(numRecords) ?? _roleManager.Roles.Where(predicate).Take(numRecords);
        }

        public override IQueryable<Role> GetAll()
        {
            return _roleManager.Roles.AsQueryable();
        }

        public override async Task<Role> GetByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        // CRUD
        public override async Task<string> CreateAsync(Role entity)
        {
            await _roleManager.CreateAsync(entity);
            await SaveChangesAsync();
            return entity.DbId;
        }

        public override async Task<string> UpdateAsync(Role entity)
        {
            await _roleManager.UpdateAsync(entity);
            await SaveChangesAsync();
            return entity.DbId;
        }

        public override async Task<string> RemoveByIdAsync(string guid)
        {
            var entity = await GetByIdAsync(guid);
            if (entity == null)
            {
                return default(string);
            }
            var ret = await _roleManager.DeleteAsync(entity);
            await SaveChangesAsync();

            return ret.Succeeded ? entity.DbId : default(string);
        }

        public override void Dispose()
        {
            _context?.Dispose();
            _roleManager.Dispose();
        }

        // IRoleRepository stuff
        public async Task<Role> GetByRoleNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }
    }
}
