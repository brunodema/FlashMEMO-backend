using Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class FlashMEMOContext : DbContext
    {
        public FlashMEMOContext(DbContextOptions<FlashMEMOContext> options) : base(options)
        { 
        }
        public DbSet<News> News { get; set; }
    }

    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }
    }
}

//https://balta.io/artigos/aspnet-5-autenticacao-autorizacao-bearer-jwt
//https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/workflows/new-database#1-create-the-application
//https://www.c-sharpcorner.com/article/authentication-and-authorization-in-asp-net-core-web-api-with-json-web-tokens/
