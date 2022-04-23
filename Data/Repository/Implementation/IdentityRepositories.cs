using Data.Context;
using Data.Models.Implementation;
using Microsoft.AspNetCore.Identity;

namespace Data.Repository.Implementation
{
    public class ApplicationUserRepository : GenericUserRepository<ApplicationUser, FlashMEMOContext>
    {
        public ApplicationUserRepository(FlashMEMOContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }
    }

    public class RoleRepository : GenericRoleRepository<ApplicationRole, string, FlashMEMOContext>
    {
        public RoleRepository(FlashMEMOContext context, RoleManager<ApplicationRole> roleManager) : base(context, roleManager)
        {
        }
    }
}
