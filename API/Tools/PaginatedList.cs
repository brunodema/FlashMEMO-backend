using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Tools
{
    /// <summary>
    /// A paginated list contains information about a paginated query, containing information such as total results, page index, etc. Depending on the API used, the amount of results returned might be extreme (in the order of billions). To follow the same practices as the API owners of such services, the type of the variable associated with this information can be set to something like 'string', which doesn't depend on bit-size.
    /// </summary>
    /// <typeparam name="T">Object type returned by the query.</typeparam>
    /// <typeparam name="QuantityType">Type used to represent information such as page index, or total amount of results. Usual types for this can be 'string' or 'int'.</typeparam>
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
