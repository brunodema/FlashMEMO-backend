using System;
using System.Linq;
using Data.Models;
using Data.Roles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public class DbSeeder
    {
        public static void InitializeDatabase(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<FlashMEMOContext>();
            var roleStore = new RoleStore<ApplicationRole>(context);

            if (context.Roles.Any()) return;

            foreach (string role in FlashMEMORoles.Roles)
            {
                if (!context.Roles.Any(r => r.Name == role))
                {
                    roleStore.CreateAsync(new ApplicationRole { Name = role });
                }
            }

            context.SaveChangesAsync();
        }
    }
}
