﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Tools
{
    public interface IPaginatedList<T, QuantityType>
    {
        public IList<T> Results { get; set; }
        public QuantityType PageIndex { get; set; }
        public QuantityType TotalPages { get; set; }
        public int ResultSize { get; set; }
        public QuantityType TotalAmount { get; set; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }
    }

    public class LargePaginatedList<T> : IPaginatedList<T, string>
    {
        public IList<T> Results { get; set; }
        public string PageIndex { get; set; }
        public string TotalPages { get; set; }
        public int ResultSize { get; set; }
        public string TotalAmount { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        public LargePaginatedList() { }
    }

    // taken from: https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/sort-filter-page?view=aspnetcore-5.0
    public class PaginatedList<T> : IPaginatedList<T, ulong>
    {
        public IList<T> Results { get; set; }
        public ulong PageIndex { get; set; }
        public ulong TotalPages { get; set; }
        public int ResultSize { get; set; }
        public ulong TotalAmount { get; set; }

        public PaginatedList() { }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>
            {
                PageIndex = Convert.ToUInt64(pageIndex),
                TotalPages = Convert.ToUInt64(Math.Ceiling(count / (double)pageSize)),
                TotalAmount = Convert.ToUInt64(count),
                ResultSize = items.Count,
                Results = new List<T>(items)
            };
        }
    }
}
