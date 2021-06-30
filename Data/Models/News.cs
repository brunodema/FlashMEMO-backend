using Data.Models.Interfaces;
using Data.Repository.Interfaces;
using System;
using System.Linq.Expressions;

namespace Data.Models
{
    public class News : INews
    {
        // kids, ALWAYS set default values for properties in the database. This will avoid errors when dealing with ICollection items + LINQ, especially for cases when the current member of the lambda being analyzed is a "null" item. 
        public News() { }
        public Guid NewsID { get; set; }
        public string Title { get; set; } = "";
        public string Subtitle { get; set; } = "";
        public string ThumbnailPath { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public Guid GetId()
        {
            return NewsID;
        }
    }
}
