using System;
using System.Linq;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public class FlashMEMORoles
    {
        public static readonly string[] Roles =
        {
            "admin",
            "user"
        };
    }
    public class DbSeeder
    {
        public static void InitializeDatabase(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<FlashMEMOContext>();
            var roleStore = new RoleStore<ApplicationRole>(context);
            var userStore = new UserStore<ApplicationUser>(context);

            if (!context.Roles.Any())
            {
                foreach (string role in FlashMEMORoles.Roles)
                {
                    if (!context.Roles.Any(r => r.Name == role))
                    {
                        roleStore.CreateAsync(new ApplicationRole { Name = role });
                    }
                }
            }
            if (!context.Users.Any())
            {
                var user = new ApplicationUser
                {
                    UserName = "sysadmin",
                    Email = "sysadmin@flashmemo.com"
                };
                user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, "Flashmemo@123");
                userStore.CreateAsync(user);
            }

            context.SaveChangesAsync();
        }
    }
}
