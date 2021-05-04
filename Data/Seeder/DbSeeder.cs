using System;
using System.Linq;
using System.Threading.Tasks;
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
        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
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
                var news1 = new News
                {
                    Title = "This is a sample news!",
                    Subtitle = "This is a sample subtitle!",
                    Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.",
                    ThumbnailPath = "~/assets/features/flashmemo_dummy1.jpg",
                    CreationDate = DateTime.Now,
                    LastUpdated = DateTime.Now,
                };
                await context.News.AddAsync(news1);
                var news2 = new News
                {
                    Title = "This is a sample news!",
                    Subtitle = "This is a sample subtitle!",
                    ThumbnailPath = "~/assets/features/flashmemo_dummy2.jpg",
                    Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.",
                    CreationDate = DateTime.Now,
                    LastUpdated = DateTime.Now,
                };
                await context.News.AddAsync(news2);
                var news3 = new News
                {
                    Title = "This is a sample news! Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.",
                    Subtitle = "This is a sample subtitle!",
                    ThumbnailPath = "~/assets/features/flashmemo_dummy3.jpg",
                    Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.",
                    CreationDate = DateTime.Now,
                    LastUpdated = DateTime.Now,
                };
                await context.News.AddAsync(news3);
                var news4 = new News
                {
                    Title = "This is a sample news!",
                    Subtitle = "This is a sample subtitle! Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.",
                    ThumbnailPath = "~/assets/features/flashmemo_dummy4.jpg",
                    Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.",
                    CreationDate = DateTime.Now,
                    LastUpdated = DateTime.Now,
                };
                await context.News.AddAsync(news4);
            }

            await context.SaveChangesAsync();
        }
    }
}
