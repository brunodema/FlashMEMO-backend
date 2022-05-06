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
using static Data.Models.Implementation.StaticModels;
using Microsoft.EntityFrameworkCore;

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
            _userStore = new UserStore<Models.Implementation.ApplicationUser>(_context);
            _seederPath = seederPath;
        }

        private async Task SeedUsers(bool forceBootstrap)
        {
            if (forceBootstrap)
            {
                var userSeeder = JsonConvert.DeserializeObject<IdentityUser[]>(File.ReadAllText($"{_seederPath}/Users.json"));
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
                // maybe I can change this in the future to: newsSeeder.Select(s => {return s.CreationDate.ToUniversalTime(); }).ToList(); ???
                newsSeeder.Select(s => { s.CreationDate = s.CreationDate.ToUniversalTime(); return s; }).ToList();
                newsSeeder.Select(s => { s.LastUpdated = s.CreationDate.ToUniversalTime(); return s; }).ToList();
                await _context.AddRangeAsync(newsSeeder);
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedLanguages(bool forceBootstrap)
        {
            if (forceBootstrap)
            {
                // samples generated with generatedata.com
                var languageSeeder = JsonConvert.DeserializeObject<Language[]>(File.ReadAllText($"{_seederPath}/Languages.json"));
                await _context.AddRangeAsync(languageSeeder);
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedDecks(bool forceBootstrap)
        {
            if (forceBootstrap)
            {
                // samples generated with generatedata.com
                var deckSeeder = JsonConvert.DeserializeObject<Deck[]>(File.ReadAllText($"{_seederPath}/Decks.json"));
                deckSeeder.Select(s => { s.CreationDate = s.CreationDate.ToUniversalTime(); return s; }).ToList();
                deckSeeder.Select(s => { s.LastUpdated = s.LastUpdated.ToUniversalTime(); return s; }).ToList();
                deckSeeder.Select(s => { s.OwnerId = s.OwnerId.ToLower(); return s; }).ToList();
                await _context.AddRangeAsync(deckSeeder);
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedFlashcards(bool forceBootstrap)
        {
            if (forceBootstrap)
            {
                // samples generated with generatedata.com
                var flashcardSeeder = JsonConvert.DeserializeObject<Flashcard[]>(File.ReadAllText($"{_seederPath}/Flashcard.json"));
                flashcardSeeder.Select(s => { s.CreationDate = s.CreationDate.ToUniversalTime(); return s; }).ToList();
                flashcardSeeder.Select(s => { s.DueDate = s.DueDate.ToUniversalTime(); return s; }).ToList();
                flashcardSeeder.Select(s => { s.LastUpdated = s.CreationDate.ToUniversalTime(); return s; }).ToList();
                await _context.AddRangeAsync(flashcardSeeder);
            }
            await _context.SaveChangesAsync();
        }

        public async Task InitializeDatabaseAsync(bool forceBootstrap = false)
        {
            _context.Database.Migrate();

            //await SeedRoles(forceBootstrap ? true : !_context.Roles.Any());
            //await SeedNews(forceBootstrap ? true : !_context.News.Any());
            //await SeedLanguages(forceBootstrap ? true : !_context.Languages.Any());
            //SeedUsers(forceBootstrap ? true : !_context.Users.Any()).Wait();
            //SeedDecks(forceBootstrap ? true : !_context.Decks.Any()).Wait();
            //SeedFlashcards(forceBootstrap ? true : !_context.Flashcards.Any()).Wait();
        }
    }
}
