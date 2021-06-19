using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Tools
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
        public Expression<Func<TEntity, object>> ColumnToSort { get; set; } = null;
    }
    public class NewsSortOptions : GenericSortOptions<News>
    {
        public static class ColumnOptions
        {
            public const string SUBTITLE = "subtitle";
            public const string DATE = "date";
        }
        public NewsSortOptions(SortType sortType, string columnToSort = "title")
        {
            SortType = sortType;
            switch (columnToSort)
            {
                case ColumnOptions.SUBTITLE:
                    ColumnToSort = news => news.Subtitle;
                    break;
                case ColumnOptions.DATE:
                    ColumnToSort = news => news.CreationDate;
                    break;
                default: // default will be title
                    ColumnToSort = news => news.Title;
                    break;
            }
        }
    }

    public class ApplicationUserSortOptions : GenericSortOptions<ApplicationUser>
    {
        public static class ColumnOptions
        {
            public const string EMAIL = "email";
        }
        public ApplicationUserSortOptions(SortType sortType, string columnToSort = "")
        {
            SortType = sortType;
            switch (columnToSort)
            {
                case ColumnOptions.EMAIL:
                    ColumnToSort = user => user.Email;
                    break;
                default: // default will be username
                    ColumnToSort = user => user.UserName;
                    break;
            }
        }
    }

    public class RoleSortOptions : GenericSortOptions<ApplicationRole>
    {
        public static class ColumnOptions
        {
            public const string NAME = "name"; // will not be used for now
        }
        public RoleSortOptions(SortType sortType, string columnToSort = "name")
        {
            SortType = sortType;
            ColumnToSort = role => role.Name;
        }
    }
}
