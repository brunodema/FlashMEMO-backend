using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Context;
using Data.Tools.Implementation;
using Data.Repository.Abstract;
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
