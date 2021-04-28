using Data.Models;
using System.Data.Entity;
using System;

namespace Data
{
    public class FlashMEMOContext : DbContext
    {
        public FlashMEMOContext() : base()
        { 
        }
        public DbSet<News> News { get; set; }
    }
}

//https://balta.io/artigos/aspnet-5-autenticacao-autorizacao-bearer-jwt
//https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/workflows/new-database#1-create-the-application
