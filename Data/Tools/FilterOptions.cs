using Data.Models.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using static Data.Models.Implementation.StaticModels;

namespace Data.Tools.Filtering
{
    public interface IQueryFilterOptions<TEntity> where TEntity : class
    {
        /// <summary>
        /// Returns the full URL to be used for the query, according to TEntity's properties and specific filters (ex: FromDate <= x <= ToDate)
        /// </summary>
        /// <returns>Full URL to be used for the query.</returns>
        public string BuildQueryURL();

        public IEnumerable<TEntity> GetFilteredResults(IQueryable<TEntity> elements);
    }

    public class LanguageFilterOptions : IQueryFilterOptions<Language>
    {
        public string ISOCode { get; set; }
        public string Name { get; set; }

        public string BuildQueryURL()
        {
            string queryParams = "";

            if (ISOCode != null) queryParams = String.Concat(queryParams, $"&LanguageISOCode={ISOCode}");
            if (Name != null) queryParams = String.Concat(queryParams, $"&Name={Name}");

            return queryParams;
        }

        public IEnumerable<Language> GetFilteredResults(IQueryable<Language> elements)
        {
            if (ISOCode != null)
            {
                elements = elements.Where(x => x.ISOCode.Contains(ISOCode));
            }
            if (Name != null)
            {
                elements = elements.Where(x => x.Name.Contains(Name));
            }

            return elements;
        }
    }

    public class RoleFilterOptions : IQueryFilterOptions<ApplicationRole>
    {
        public string Name { get; set; }

        public string BuildQueryURL()
        {
            string queryParams = "";

            if (Name != null) queryParams = String.Concat(queryParams, $"&Name={Name}");

            return queryParams;
        }

        public IEnumerable<ApplicationRole> GetFilteredResults(IQueryable<ApplicationRole> elements)
        {
            if (Name != null)
            {
                elements = elements.Where(x => x.Name.Contains(Name));
            }

            return elements;
        }
    }

    public class UserFilterOptions : IQueryFilterOptions<ApplicationUser>
    {
        public string Email { get; set; }
        public string Username { get; set; }

        public string BuildQueryURL()
        {
            string queryParams = "";

            if (Email != null) queryParams = String.Concat(queryParams, $"&Email={Email}");
            if (Username != null) queryParams = String.Concat(queryParams, $"&Username={Username}");

            return queryParams;
        }

        public IEnumerable<ApplicationUser> GetFilteredResults(IQueryable<ApplicationUser> elements)
        {
            if (Email != null)
            {
                elements = elements.Where(x => x.Email.Contains(Email));
            }
            if (Username != null)
            {
                elements = elements.Where(x => x.UserName.Contains(Username));
            }

            return elements;
        }
    }

    public class DeckFilterOptions : IQueryFilterOptions<Deck>
    {
        public string OwnerEmail { get; set; } = null;
        public string LanguageCode { get; set; } = null;
        public string Name { get; set; } = null;
        public string Description { get; set; } = null;
        public DateTime? FromCreationDate { get; set; } = null;
        public DateTime? ToCreationDate { get; set; } = null;
        public DateTime? FromLastUpdated { get; set; } = null;
        public DateTime? ToLastUpdated { get; set; } = null;

        public string BuildQueryURL()
        {
            string queryParams = "";

            if (OwnerEmail != null) queryParams = String.Concat(queryParams, $"&OwnerEmail={OwnerEmail}");
            if (LanguageCode != null) queryParams = String.Concat(queryParams, $"&LanguageCode={LanguageCode}");
            if (Name != null) queryParams = String.Concat(queryParams, $"&Name={Name}");
            if (Description != null) queryParams = String.Concat(queryParams, $"&Description={Description}");
            if (FromCreationDate != null) queryParams = String.Concat(queryParams, $"&FromCreationDate={FromCreationDate.Value.ToString("yyyy-MM-dd")}");
            if (ToCreationDate != null) queryParams = String.Concat(queryParams, $"&ToCreationDate={ToCreationDate.Value.ToString("yyyy-MM-dd")}");
            if (FromLastUpdated != null) queryParams = String.Concat(queryParams, $"&FromLastUpdated={FromLastUpdated.Value.ToString("yyyy-MM-dd")}");
            if (ToLastUpdated != null) queryParams = String.Concat(queryParams, $"&ToLastUpdated={ToLastUpdated.Value.ToString("yyyy-MM-dd")}");

            return queryParams;
        }

        public IEnumerable<Deck> GetFilteredResults(IQueryable<Deck> elements)
        {
            var processedFromCreationDate = FromCreationDate ?? DateTime.MinValue;
            var processedToCreationDate = ToCreationDate ?? DateTime.MaxValue;
            elements = elements.Where(x => x.CreationDate >= processedFromCreationDate && x.CreationDate <= processedToCreationDate);
            var processedFromLastUpdated = FromLastUpdated ?? DateTime.MinValue;
            var processedToLastUpdated = ToLastUpdated ?? DateTime.MaxValue;
            elements = elements.Where(x => x.LastUpdated >= processedFromLastUpdated && x.LastUpdated <= processedToLastUpdated);

            if (OwnerEmail != null)
            {
                elements = elements.Where(x => x.Owner.Email.Contains(OwnerEmail));
            }
            if (LanguageCode != null)
            {
                elements = elements.Where(x => x.Language.ISOCode.Contains(LanguageCode));
            }
            if (Name != null)
            {
                elements = elements.Where(x => x.Name.Contains(Name));
            }
            if (Description != null)
            {
                elements = elements.Where(x => x.Description.Contains(Description));
            }

            return elements;
        }
    }

    public class FlashcardFilterOptions : IQueryFilterOptions<Flashcard>
    {
        public Guid FlashcardID { get; set; } = new Guid();

        public int Level { get; set; } = -1;
        public string Answer { get; set; } = "";
        public DateTime? FromCreationDate { get; set; } = null;
        public DateTime? ToCreationDate { get; set; } = null;
        public DateTime? FromLastUpdated { get; set; } = null;
        public DateTime? ToLastUpdated { get; set; } = null;
        public DateTime? FromDueDate { get; set; } = null;
        public DateTime? ToDueDate { get; set; } = null;

        public string BuildQueryURL()
        {
            string queryParams = "";

            if (Level > -1) queryParams = String.Concat(queryParams, $"&Level={Level}");
            if (!String.IsNullOrEmpty(Answer)) queryParams = String.Concat(queryParams, $"&Answer={Answer}");
            if (FromCreationDate != null) queryParams = String.Concat(queryParams, $"&FromCreationDate={FromCreationDate.Value.ToString("yyyy-MM-dd")}");
            if (ToCreationDate != null) queryParams = String.Concat(queryParams, $"&ToCreationDate={ToCreationDate.Value.ToString("yyyy-MM-dd")}");
            if (FromLastUpdated != null) queryParams = String.Concat(queryParams, $"&FromLastUpdated={FromLastUpdated.Value.ToString("yyyy-MM-dd")}");
            if (ToLastUpdated != null) queryParams = String.Concat(queryParams, $"&ToLastUpdated={ToLastUpdated.Value.ToString("yyyy-MM-dd")}");
            if (FromDueDate != null) queryParams = String.Concat(queryParams, $"&FromDueDate={FromDueDate.Value.ToString("yyyy-MM-dd")}");
            if (ToDueDate != null) queryParams = String.Concat(queryParams, $"&ToDueDate={ToDueDate.Value.ToString("yyyy-MM-dd")}");

            return queryParams;
        }

        public IEnumerable<Flashcard> GetFilteredResults(IQueryable<Flashcard> elements)
        {
            var processedFromCreationDate = FromCreationDate ?? DateTime.MinValue;
            var processedToCreationDate = ToCreationDate ?? DateTime.MaxValue;
            elements = elements.Where(x => x.CreationDate >= processedFromCreationDate && x.CreationDate <= processedToCreationDate);
            var processedFromLastUpdated = FromLastUpdated ?? DateTime.MinValue;
            var processedToLastUpdated = ToLastUpdated ?? DateTime.MaxValue;
            elements = elements.Where(x => x.LastUpdated >= processedFromLastUpdated && x.LastUpdated <= processedToLastUpdated);
            var processedFromDueDate = FromDueDate ?? DateTime.MinValue;
            var processedToDueDate = ToDueDate ?? DateTime.MaxValue;
            elements = elements.Where(x => x.DueDate >= processedFromDueDate && x.DueDate <= processedToDueDate);

            if (Level > -1)
            {
                elements = elements.Where(x => x.Level == Level);
            }
            if (String.IsNullOrEmpty(Answer))
            {
                elements = elements.Where(x => x.Answer == Answer);
            }

            return elements;
        }
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
            var processedFromDate = FromDate ?? DateTime.MinValue.Date;
            var processedToDate = ToDate ?? DateTime.MaxValue.Date;

            // used tp focus on the date itself, instead of specifics (hour, minute, etc)
            processedFromDate = new DateTime(processedFromDate.Year, processedFromDate.Month, processedFromDate.Day, 0, 0, 0);
            processedToDate = new DateTime(processedToDate.Year, processedFromDate.Month, processedFromDate.Day, 23, 59, 59);


            elements = elements.Where(x => x.CreationDate >= processedFromDate.ToUniversalTime() && x.CreationDate <= processedToDate.ToUniversalTime());

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
