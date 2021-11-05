﻿using Data.Context;
using Data.Models.Implementation;
using Data.Repository.Abstract;
using Data.Tools.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository.Implementation
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser, string, FlashMEMOContext>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUserRepository(FlashMEMOContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
        }
        public override IEnumerable<ApplicationUser> SearchAndOrderAsync(Expression<Func<ApplicationUser, bool>> predicate, GenericSortOptions<ApplicationUser> sortOptions, int numRecords)
        {
            return sortOptions.GetSortedResults(_dbset.AsNoTracking().Where(predicate)).Take(numRecords);
        }
        public override async Task<IEnumerable<ApplicationUser>> SearchAllAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _userManager.Users.AsNoTracking().Where(predicate).ToListAsync();
        }
        public override async Task<ApplicationUser> SearchFirstAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        public override IQueryable<ApplicationUser> GetAll()
        {
            return _userManager.Users.AsQueryable();
        }
        public override async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }
        // CRUD
        // Note: this is the intended method when creating a user, since this 'override' uses the 'UserManager' instead of context.
        public override async Task<IdentityResult> CreateAsync(ApplicationUser entity)
        {
            return await _userManager.CreateAsync(entity);
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
