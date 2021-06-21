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
    public interface ISortOptions<TEntity>
    {
        public SortType SortType { get; set; }
        public Expression<Func<TEntity, object>> ColumnToSort { get; set; }
        public ISortOptions<TEntity> GetSortOptions(SortType sortType, string columnToSort);
    }
    public class NewsSortOptions : ISortOptions<News>
    {
        public SortType SortType { get; set; } = SortType.None;
        public Expression<Func<News, object>> ColumnToSort { get; set; } = null;
        public static class ColumnOptions
        {
            public const string SUBTITLE = "subtitle";
            public const string DATE = "date";
        }
        public ISortOptions<News> GetSortOptions(SortType sortType, string columnToSort = "title")
        {
            var sortOptions = new NewsSortOptions();
            sortOptions.SortType = sortType;
            switch (columnToSort)
            {
                case ColumnOptions.SUBTITLE:
                    sortOptions.ColumnToSort = news => news.Subtitle;
                    break;
                case ColumnOptions.DATE:
                    sortOptions.ColumnToSort = news => news.CreationDate;
                    break;
                default: // default will be title
                    sortOptions.ColumnToSort = news => news.Title;
                    break;
            }
            return sortOptions;
        }
    }

    public class ApplicationUserSortOptions : ISortOptions<ApplicationUser>
    {
        public SortType SortType { get; set; } = SortType.None;
        public Expression<Func<ApplicationUser, object>> ColumnToSort { get; set; } = null;
        public static class ColumnOptions
        {
            public const string EMAIL = "email";
        }
        public ISortOptions<ApplicationUser> GetSortOptions(SortType sortType, string columnToSort = "username")
        {
            var sortOptions = new ApplicationUserSortOptions();
            sortOptions.SortType = sortType;
            switch (columnToSort)
            {
                case ColumnOptions.EMAIL:
                    sortOptions.ColumnToSort = user => user.Email;
                    break;
                default: // default will be username
                    sortOptions.ColumnToSort = user => user.UserName;
                    break;
            }
            return sortOptions;
        }
    }

    public class RoleSortOptions : ISortOptions<ApplicationRole>
    {
        public SortType SortType { get; set; } = SortType.None;
        public Expression<Func<ApplicationRole, object>> ColumnToSort { get; set; } = null;
        public static class ColumnOptions
        {
            public const string NAME = "name"; // will not be used for now
        }
        public ISortOptions<ApplicationRole> GetSortOptions(SortType sortType, string columnToSort = "name")
        {
            return new RoleSortOptions
            {
                SortType = sortType,
                ColumnToSort = role => role.Name
            };
        }
    }
}
