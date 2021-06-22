﻿using Data.Models;
using Data.Tools.Implementations;


namespace Data.Tools
{
     public class NewsSortOptions : GenericSortOptions<News>
    {
        public static class ColumnOptions
        {
            public const string SUBTITLE = "subtitle";
            public const string DATE = "date";
        }
        public NewsSortOptions(SortType sortType = SortType.None, string columnToSort = "") : base(sortType, columnToSort) { }
        public override void DetermineColumnToSort(string columnToSort = "title")
        {
            ColumnToSort = columnToSort switch
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
        public override void DetermineColumnToSort(string columnToSort = "title")
        {
            ColumnToSort = columnToSort switch
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
        public override void DetermineColumnToSort(string columnToSort = "name")
        {
            ColumnToSort = role => role.Name;
        }
    }
}
