using Microsoft.AspNetCore.Identity;
using Data.Context;
using Data.Models.Implementation;

namespace Data.Repository.Implementation
{
    public class RoleRepository : GenericRoleRepository<ApplicationRole, string, FlashMEMOContext>
    {
        public RoleRepository(FlashMEMOContext context, RoleManager<ApplicationRole> roleManager) : base(context, roleManager)
        {
        }
    }
}
