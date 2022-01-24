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
        private FlashMEMOContext _context { get; set; }
        private RoleStore<ApplicationRole> _roleStore { get; set; }
        private UserStore<ApplicationUser> _userStore { get; set; }
        private string _seederPath { get; set; }

        public DbSeeder(IServiceProvider serviceProvider, string seederPath)
        {
            _context = serviceProvider.GetService<FlashMEMOContext>();
            _roleStore = new RoleStore<ApplicationRole>(_context);
            _userStore = new UserStore<ApplicationUser>(_context);
            _seederPath = seederPath;
        }

        private async Task SeedUsers(bool forceBootstrap)
        {
            if (forceBootstrap)
            {
                var user = new ApplicationUser
                {
                    UserName = "sysadmin",
                    NormalizedUserName = "SYSADMIN",
                    Email = "sysadmin@flashmemo.com",
                    NormalizedEmail = "SYSADMIN@FLASHMEMO.COM",

                };
                user.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(user, "Flashmemo@123");
                await _userStore.CreateAsync(user);

                // samples generated with generatedata.com
                var userSeeder = JsonConvert.DeserializeObject<News[]>(File.ReadAllText($"{_seederPath}/Users.json"));
                await _context.AddRangeAsync(userSeeder);
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedRoles(bool forceBootstrap)
        {
            if (forceBootstrap)
            {
                foreach (string role in FlashMEMORoles.Roles)
                {
                    if (!_context.Roles.Any(r => r.Name == role))
                    {
                        await _roleStore.CreateAsync(new ApplicationRole { Name = role });
                    }
                }
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedNews(bool forceBootstrap)
        {
            if (forceBootstrap)
            {
                // samples generated with generatedata.com
                var newsSeeder = JsonConvert.DeserializeObject<News[]>(File.ReadAllText($"{_seederPath}/News.json"));
                // I have to apply these transforms here to avoid this exception in the new version of the pgsql library: "Cannot write DateTime with Kind=UTC to PostgreSQL type 'timestamp without time zone'"
                newsSeeder.Select(s => { s.CreationDate = s.CreationDate.ToUniversalTime(); return s; }).ToList();
                newsSeeder.Select(s => { s.LastUpdated = s.CreationDate.ToUniversalTime(); return s; }).ToList();
                await _context.AddRangeAsync(newsSeeder);
            }
            await _context.SaveChangesAsync();
        }

        public async Task InitializeDatabaseAsync(bool forceBootstrap = false)
        {
            await SeedRoles(forceBootstrap ? false : !_context.Users.Any());
            await SeedUsers(forceBootstrap ? false : !_context.Roles.Any());
            await SeedNews(forceBootstrap ? false : !_context.News.Any());
        }
    }
}
