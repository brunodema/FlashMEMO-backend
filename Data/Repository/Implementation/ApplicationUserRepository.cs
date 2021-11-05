using Data.Context;
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
    public class ApplicationUserRepository : GenericUserRepository<ApplicationUser, string, FlashMEMOContext>
    {
        public ApplicationUserRepository(FlashMEMOContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }
    }
}
