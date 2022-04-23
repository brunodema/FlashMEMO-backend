using Data.Context;
using Data.Models.Implementation;
using Data.Repository.Abstract;
using Data.Tools.Exceptions.Repository;
using Data.Tools.Filtering;
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
        Task<bool> CheckPasswordAsync(TUserType user, string password);
    }

    public class ApplicationUserRepository : GenericRepository<ApplicationUser, string, FlashMEMOContext>, IUserRepository<ApplicationUser>
    {
        protected UserManager<ApplicationUser> _userManager;

        public ApplicationUserRepository(FlashMEMOContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public override IEnumerable<ApplicationUser> SearchAndOrder(Expression<Func<ApplicationUser, bool>> predicate, GenericSortOptions<ApplicationUser> sortOptions = null, int numRecords = 10)
        {
            if (numRecords < 0)
            {
                return sortOptions?.GetSortedResults(_userManager.Users.Where(predicate)) ?? _userManager.Users.Where(predicate);
            }
            return sortOptions?.GetSortedResults(_userManager.Users.Where(predicate)).Take(numRecords) ?? _userManager.Users.Where(predicate).Take(numRecords);
        }

        public override IQueryable<ApplicationUser> GetAll()
        {
            return _userManager.Users.AsQueryable();
        }

        public override async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        // CRUD
        public override async Task<string> CreateAsync(ApplicationUser entity)
        {
            await _userManager.CreateAsync(entity);
            await SaveChangesAsync();
            return entity.DbId;
        }

        public override async Task<string> UpdateAsync(ApplicationUser entity)
        {
            await _userManager.UpdateAsync(entity);
            await SaveChangesAsync();
            return entity.DbId;
        }

        public override async Task<string> RemoveByIdAsync(string guid)
        {
            var entity = await GetByIdAsync(guid);
            if (entity == null)
            {
                throw new ObjectNotFoundWithId<string>(guid);
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

        public async Task SetInitialPasswordAsync(ApplicationUser user, string newPassword)
        {
            await _userManager.AddPasswordAsync(user, newPassword);
        }

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }

    public class RoleRepository : GenericRoleRepository<ApplicationRole, string, FlashMEMOContext>
    {
        public RoleRepository(FlashMEMOContext context, RoleManager<ApplicationRole> roleManager) : base(context, roleManager)
        {
        }
    }
}
