using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Tools
{
    // taken from: https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/sort-filter-page?view=aspnetcore-5.0
    public class PaginatedList<T>
    {
        public IList<T> Results { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int ResultSize { get; set; }
        public UInt32 TotalAmount { get; set; }

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
                PageIndex = pageIndex,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                TotalAmount = Convert.ToUInt32(count),
                ResultSize = items.Count,
                Results = new List<T>(items)
            };
        }
    }
}
