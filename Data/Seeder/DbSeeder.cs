using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Data.Context;
using System.IO;
using Data.Models.Implementation;
using Newtonsoft.Json;

namespace Data.Seeder
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
        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider, string seederPath)
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
                        await roleStore.CreateAsync(new ApplicationRole { Name = role });
                    }
                }
            }
            if (!context.Users.Any())
            {
                var user = new ApplicationUser
                {
                    UserName = "sysadmin",
                    NormalizedUserName = "SYSADMIN",
                    Email = "sysadmin@flashmemo.com",
                    NormalizedEmail = "SYSADMIN@FLASHMEMO.COM",
                    
                };
                user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, "Flashmemo@123");
                await userStore.CreateAsync(user);
            }
            if (!context.News.Any())
            {
                // samples generated with generatedata.com
                var newsSeeder = JsonConvert.DeserializeObject<News[]>(File.ReadAllText($"{seederPath}/News.json"));
                await context.AddRangeAsync(newsSeeder);
            }

            await context.SaveChangesAsync();
        }
    }
}
