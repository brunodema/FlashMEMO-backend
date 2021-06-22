using Data.Models;
using Data.Tools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Tools
{
    public class NewsFilterOptions : IQueryFilterOptions<News>
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public string Title { get; set; } = null;
        public string Subtitle { get; set; } = null;
        public string Content { get; set; } = null;

        public IEnumerable<News> GetFilteredResults(IQueryable<News> elements)
        {
            var processedFromDate = FromDate ?? DateTime.MinValue;
            var processedToDate = ToDate ?? DateTime.MaxValue;
            elements = elements.Where(x => x.CreationDate >= processedFromDate && x.CreationDate <= processedToDate);

            if (Title != null)
            {
                elements = elements.Where(x => x.Title.Contains(Title));
            }
            if (Subtitle != null)
            {
                elements = elements.Where(x => x.Subtitle.Contains(Subtitle));
            }
            if (Content != null)
            {
                elements = elements.Where(x => x.Content.Contains(Content));
            }

            return elements;
        }
    }
}
