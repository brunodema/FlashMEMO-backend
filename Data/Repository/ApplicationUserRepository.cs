using Data;
using Data.Interfaces;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repository
{
    //public abstract class AuthRepository<TManager, IdentityModel> : IBaseRepository<IdentityModel, FlashMEMOContext> where IdentityModel : class
    //{
    //    private readonly TManager _manager;

    //    public AuthRepository(FlashMEMOContext context, TManager manager) : base(context)
    //    {
    //        _manager = manager;
    //    }
    //    public virtual async Task<IEnumerable<IdentityModel>> SearchAndOrderAsync<TKey>(Expression<Func<IdentityModel, bool>> predicate, SortType sortType, Expression<Func<IdentityModel, TKey>> columnToSort, int numRecords)
    //    {
    //        if (sortType == SortType.Ascending)
    //        {
    //            return await _manager.Users.AsNoTracking().Where(predicate).OrderBy(columnToSort).Take(numRecords).ToListAsync();
    //        }
    //        return await _manager.Users.AsNoTracking().Where(predicate).OrderByDescending(columnToSort).Take(numRecords).ToListAsync();

    //    }
    //    public virtual async Task<IEnumerable<IdentityModel>> SearchAllAsync(Expression<Func<IdentityModel, bool>> predicate)
    //    {
    //        return await _manager.Users.AsNoTracking().Where(predicate).ToListAsync();
    //    }
    //    public virtual async Task<IdentityModel> SearchFirstAsync(Expression<Func<IdentityModel, bool>> predicate)
    //    {
    //        return await _manager.Users.AsNoTracking().FirstOrDefaultAsync(predicate);
    //    }
    //    public virtual async Task<ICollection<IdentityModel>> GetAllAsync()
    //    {
    //        return await _manager.Users.ToListAsync();
    //    }
    //    public virtual async Task<IdentityModel> GetByIdAsync(Guid id)
    //    {
    //        return await _manager.FindByIdAsync(id.ToString());
    //    }
    //    // CRUD
    //    // Note: this is the intended method when creating a user, since this 'override' uses the 'UserManager' instead of context.
    //    public async Task<IdentityResult> CreateUserAsync(IdentityModel entity, string encryptedPassword)
    //    {
    //        return await _manager.CreateAsync(entity, encryptedPassword);
    //    }
    //    public virtual async Task<IdentityResult> UpdateAsync(IdentityModel entity)
    //    {
    //        return await _manager.UpdateAsync(entity);
    //    }
    //    public virtual async Task<IdentityResult> RemoveAsync(IdentityModel entity)
    //    {
    //        return await _manager.DeleteAsync(entity);
    //    }
    //    public virtual void Dispose()
    //    {
    //        _manager?.Dispose();
    //        base.Dispose();
    //    }
    }


public class ApplicationUserRepository : BaseRepository<ApplicationUser, FlashMEMOContext>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public ApplicationUserRepository(FlashMEMOContext context, UserManager<ApplicationUser> userManager) : base(context)
    {
        _userManager = userManager;
    }
    public override async Task<IEnumerable<ApplicationUser>> SearchAndOrderAsync<TKey>(Expression<Func<ApplicationUser, bool>> predicate, SortType sortType, Expression<Func<ApplicationUser, TKey>> columnToSort, int numRecords)
    {
        if (sortType == SortType.Ascending)
        {
            return await _userManager.Users.AsNoTracking().Where(predicate).OrderBy(columnToSort).Take(numRecords).ToListAsync();
        }
        return await _userManager.Users.AsNoTracking().Where(predicate).OrderByDescending(columnToSort).Take(numRecords).ToListAsync();

    }
    public override async Task<IEnumerable<ApplicationUser>> SearchAllAsync(Expression<Func<ApplicationUser, bool>> predicate)
    {
        return await _userManager.Users.AsNoTracking().Where(predicate).ToListAsync();
    }
    public override async Task<ApplicationUser> SearchFirstAsync(Expression<Func<ApplicationUser, bool>> predicate)
    {
        return await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(predicate);
    }
    public override async Task<ICollection<ApplicationUser>> GetAllAsync()
    {
        return await _userManager.Users.ToListAsync();
    }
    public override async Task<ApplicationUser> GetByIdAsync(Guid id)
    {
        return await _userManager.FindByIdAsync(id.ToString());
    }
    // CRUD
    // Note: this is the intended method when creating a user, since this 'override' uses the 'UserManager' instead of context.
    public async Task<IdentityResult> CreateUserAsync(ApplicationUser entity, string encryptedPassword)
    {
        return await _userManager.CreateAsync(entity, encryptedPassword);
    }
    public override async Task<IdentityResult> UpdateAsync(ApplicationUser entity)
    {
        return await _userManager.UpdateAsync(entity);
    }
    public override async Task<IdentityResult> RemoveAsync(ApplicationUser entity)
    {
        return await _userManager.DeleteAsync(entity);
    }
    public override void Dispose()
    {
        _userManager?.Dispose();
        base.Dispose();
    }
}
