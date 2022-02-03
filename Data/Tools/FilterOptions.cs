using Data.Models.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Tools.Filtering
{
    public interface IQueryFilterOptions<TEntiy> where TEntiy : class
    {
        /// <summary>
        /// Returns the full URL to be used for the query, according to TEntity's properties and specific filters (ex: FromDate <= x <= ToDate)
        /// </summary>
        /// <returns>Full URL to be used for the query.</returns>
        public string BuildQueryURL();

        public IEnumerable<TEntiy> GetFilteredResults(IQueryable<TEntiy> elements);
    }

    public class NewsFilterOptions : IQueryFilterOptions<News>
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public string Title { get; set; } = null;
        public string Subtitle { get; set; } = null;
        public string Content { get; set; } = null;

        public string BuildQueryURL()
        {
            string queryParams = "";

            if (Content != null) queryParams = String.Concat(queryParams, $"&Content={Content}");
            if (Subtitle != null) queryParams = String.Concat(queryParams, $"&Subtitle={Subtitle}");
            if (Title != null) queryParams = String.Concat(queryParams, $"&Title={Title}");
            if (FromDate != null) queryParams = String.Concat(queryParams, $"&FromDate={FromDate.Value.ToString("yyyy-MM-dd")}");
            if (ToDate != null) queryParams = String.Concat(queryParams, $"&ToDate={ToDate.Value.ToString("yyyy-MM-dd")}");

            return queryParams;
        }

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
