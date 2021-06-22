﻿using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Context;
using Data.Tools.Implementations;

namespace Data.Repository
{
    public class RoleRepository : GenericRepository<ApplicationRole, string, FlashMEMOContext>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RoleRepository(FlashMEMOContext context, RoleManager<ApplicationRole> roleManager) : base(context)
        {
            _roleManager = roleManager;
        }
        public override async Task<IEnumerable<ApplicationRole>> SearchAndOrderAsync(Expression<Func<ApplicationRole, bool>> predicate, GenericSortOptions<ApplicationRole> sortOptions, int numRecords)
        {
            if (sortOptions != null)
            {
                if (sortOptions.SortType == SortType.Ascending)
                {
                    return await _roleManager.Roles.AsNoTracking().Where(predicate).OrderBy(sortOptions.ColumnToSort).Take(numRecords).ToListAsync();
                }
                return await _roleManager.Roles.AsNoTracking().Where(predicate).OrderByDescending(sortOptions.ColumnToSort).Take(numRecords).ToListAsync();
            }
            return await _roleManager.Roles.AsNoTracking().Where(predicate).Take(numRecords).ToListAsync();

        }
        public override async Task<IEnumerable<ApplicationRole>> SearchAllAsync(Expression<Func<ApplicationRole, bool>> predicate)
        {
            return await _roleManager.Roles.AsNoTracking().Where(predicate).ToListAsync();
        }
        public override async Task<ApplicationRole> SearchFirstAsync(Expression<Func<ApplicationRole, bool>> predicate)
        {
            return await _roleManager.Roles.AsNoTracking().FirstOrDefaultAsync(predicate);
        }
        public override IQueryable<ApplicationRole> GetAll()
        {
            return _roleManager.Roles.AsQueryable();
        }
        public override async Task<ApplicationRole> GetByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }
        // CRUD
        // Note: this is the intended method when creating a user, since this 'override' uses the 'UserManager' instead of context.
        public override async Task<IdentityResult> CreateAsync(ApplicationRole entity, object[] extraParams = null)
        {
            return await _roleManager.CreateAsync(entity);
        }
        public override async Task<IdentityResult> UpdateAsync(ApplicationRole entity)
        {
            return await _roleManager.UpdateAsync(entity);
        }
        public override void Dispose()
        {
            _roleManager?.Dispose();
            base.Dispose();
        }
    }
}
