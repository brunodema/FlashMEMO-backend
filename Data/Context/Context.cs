using Data.Models.Implementation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using static Data.Models.Implementation.StaticModels;
using static Data.Tools.FlashcardTools;

namespace Data.Context
{
    public class FlashMEMOContext : IdentityDbContext<
        ApplicationUser, ApplicationRole, string,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>
    {
        private readonly string _seederPath = "../Data/Seeder";

        public FlashMEMOContext(DbContextOptions<FlashMEMOContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // following implementation follows .NET boilerplate for identity customization (https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-5.0)

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            //modelBuilder.Entity<ApplicationRole>()
            modelBuilder.Entity<ApplicationUser>()
                .HasData(JsonConvert.DeserializeObject<ApplicationUser[]>(File.ReadAllText($"{_seederPath}/Users.json")));

            modelBuilder.Entity<Language>()
                .HasData(JsonConvert.DeserializeObject<Language[]>(File.ReadAllText($"{_seederPath}/Languages.json")));

            modelBuilder.Entity<News>()
                .HasData(JsonConvert.DeserializeObject<News[]>(File.ReadAllText($"{_seederPath}/News.json"), new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc }));

            modelBuilder.Entity<Deck>()
                .HasData(JsonConvert.DeserializeObject<Deck[]>(File.ReadAllText($"{_seederPath}/Decks.json"), new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc }));

            //modelBuilder.Entity<Flashcard>()
            //    .HasData(JsonConvert.DeserializeObject<Flashcard[]>(File.ReadAllText($"{_seederPath}/Flashcards.json"), new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc }));
            modelBuilder.Entity<Flashcard>().Property(e => e.FrontContentLayout).HasConversion(
               v => v.ToString(),
               v => (FlashcardContentLayout)Enum.Parse(typeof(FlashcardContentLayout), v));
        }
        public DbSet<News> News { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<Language> Languages { get; set; }

        /*
         * Whenever something new is added/updated, do this:
         * 1. Open Package-Manager Console
         * 2. Add-Migration ${NAME_DESCRIBING_WHAT_CHANGED}
         * 3. Update-Database
         */

    }
}

//https://balta.io/artigos/aspnet-5-autenticacao-autorizacao-bearer-jwt
//https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/workflows/new-database#1-create-the-application
//https://www.c-sharpcorner.com/article/authentication-and-authorization-in-asp-net-core-web-api-with-json-web-tokens/