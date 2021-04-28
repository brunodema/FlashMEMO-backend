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
