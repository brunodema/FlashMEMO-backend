using Data.Interfaces;
using System;

namespace Data.Models
{
    public class News : INews
    {
        public News() { }
        public Guid NewsID { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string ThumbnailPath { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
