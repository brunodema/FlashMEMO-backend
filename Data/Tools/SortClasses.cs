using Data.Models.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Tools.Sorting
{
    public enum SortType
    {
        None,
        Ascending,
        Descending
    }
    public abstract class GenericSortOptions<TEntity>
    {
        public SortType SortType { get; set; } = SortType.None;
        public string ColumnToSort { get; set; } = "";
        protected Expression<Func<TEntity, object>> ColumnToSortExpression { get; set; } = null;

        public GenericSortOptions(SortType sortType = SortType.None, string columnToSort = "")
        {
            SortType = sortType;
            ColumnToSort = columnToSort;
            DetermineColumnToSortExpression(ColumnToSort);
        }
        public IEnumerable<TEntity> GetSortedResults(IQueryable<TEntity> elements)
        {
            DetermineColumnToSortExpression(ColumnToSort);
            if (SortType == SortType.Ascending)
            {
                return elements.OrderBy(ColumnToSortExpression);
            }
            else if (SortType == SortType.Descending)
            {
                return elements.OrderByDescending(ColumnToSortExpression);
            }
            return elements;
        }
        protected abstract void DetermineColumnToSortExpression(string columnToSort = "");

        public Expression<Func<TEntity, object>> GetColumnToSort()
        {
            return ColumnToSortExpression;
        }
    }

    public class FlashcardSortOptions : GenericSortOptions<Flashcard>
    {
        public static class ColumnOptions
        {
            public const string DUE_DATE = "duedate";
            public const string LEVEL = "level";
            public const string ANSWER = "answer";
            public const string CREATION_DATE = "creationdate";
            public const string LAST_UPDATED = "lastupdated";
        }
        public FlashcardSortOptions(SortType sortType = SortType.None, string columnToSort = "") : base(sortType, columnToSort) { }
        public FlashcardSortOptions() { }
        protected override void DetermineColumnToSortExpression(string columnToSort = "name")
        {
            ColumnToSortExpression = columnToSort.ToLowerInvariant() switch
            {
                ColumnOptions.DUE_DATE => f => f.DueDate,
                ColumnOptions.LEVEL => f => f.Level,
                ColumnOptions.ANSWER => f => f.Answer,
                ColumnOptions.CREATION_DATE => f => f.CreationDate,
                ColumnOptions.LAST_UPDATED => f => f.LastUpdated,
                // default will be 'duedate'
                _ => a => a.DueDate,
            };
        }
    }

    public class DeckSortOptions : GenericSortOptions<Deck>
    {
        public static class ColumnOptions
        {
            public const string NAME = "name";
            public const string OWNER = "owner";
            public const string DESCRIPTION = "description";
            public const string CREATION_DATE = "creationdate";
            public const string LAST_UPDATED = "lastupdated";
            public const string FLASHCARDS = "flashcards";
        }
        public DeckSortOptions(SortType sortType = SortType.None, string columnToSort = "") : base(sortType, columnToSort) { }
        public DeckSortOptions() { }
        protected override void DetermineColumnToSortExpression(string columnToSort = "name")
        {
            ColumnToSortExpression = columnToSort.ToLowerInvariant() switch
            {
                ColumnOptions.NAME => a => a.Name,
                ColumnOptions.OWNER => a => a.Owner.UserName,
                ColumnOptions.DESCRIPTION => a => a.Description,
                ColumnOptions.CREATION_DATE => a => a.CreationDate,
                ColumnOptions.LAST_UPDATED => a => a.LastUpdated,
                ColumnOptions.FLASHCARDS => a => a.Flashcards.Count, // trying to get the count here might lead to trouble...
                // default will be 'Name'
                _ => a => a.Name,
            };
        }
    }

    public class NewsSortOptions : GenericSortOptions<News>
    {
        public static class ColumnOptions
        {
            public const string SUBTITLE = "subtitle";
            public const string DATE = "date";
        }
        public NewsSortOptions(SortType sortType = SortType.None, string columnToSort = "") : base(sortType, columnToSort) { }
        public NewsSortOptions() { }
        protected override void DetermineColumnToSortExpression(string columnToSort = "title")
        {
            ColumnToSortExpression = columnToSort.ToLowerInvariant() switch
            {
                ColumnOptions.SUBTITLE => news => news.Subtitle,
                ColumnOptions.DATE => news => news.CreationDate,
                // default will be username
                _ => news => news.Title,
            };
        }
    }

    public class ApplicationUserSortOptions : GenericSortOptions<ApplicationUser>
    {
        public static class ColumnOptions
        {
            public const string EMAIL = "email";
        }
        public ApplicationUserSortOptions(SortType sortType = SortType.None, string columnToSort = "") : base(sortType, columnToSort) { }
        public ApplicationUserSortOptions() { }
        protected override void DetermineColumnToSortExpression(string columnToSort = "title")
        {
            ColumnToSortExpression = columnToSort.ToLowerInvariant() switch
            {
                ColumnOptions.EMAIL => user => user.Email,
                // default will be username
                _ => user => user.UserName,
            };
        }
    }

    public class RoleSortOptions : GenericSortOptions<ApplicationRole>
    {
        public static class ColumnOptions
        {
            public const string NAME = "name"; // will not be used for now
        }
        public RoleSortOptions(SortType sortType = SortType.None, string columnToSort = "") : base(sortType, columnToSort) { }
        public RoleSortOptions() { }
        protected override void DetermineColumnToSortExpression(string columnToSort = "name")
        {
            ColumnToSortExpression = role => role.Name;
        }
    }
}
