﻿using Data.Context;
using Data.Models.Implementation;
using Microsoft.AspNetCore.Identity;

namespace Data.Repository.Implementation
{
    public class ApplicationUserRepository : GenericUserRepository<ApplicationUser, string, FlashMEMOContext>
    {
        public ApplicationUserRepository(FlashMEMOContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }
    }
}
