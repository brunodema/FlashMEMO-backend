using Data.Models.Interfaces;
using Data.Repository.Interfaces;
using System;
using System.Linq.Expressions;

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

        public Guid GetId()
        {
            return NewsID;
        }

        public Expression<Func<object>> GetSortColumnFromString(string column)
        {
            switch (column)
            {
                case "subtitle":
                    return () => Subtitle;
                case "date":
                    return () =>CreationDate;
                default: // default will be title
                    return () => Title;
            }
        }
    }
}
