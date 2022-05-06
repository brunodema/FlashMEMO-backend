using Data.Tools.Sorting;
using Data.Tools.Filtering;
using Tests.Integration.Interfaces;

namespace Tests.Integration.Implementation
{
    public static class TestMessages
    {
        public class SearchParameters<TEntity> : ISearchParameters<TEntity>
            where TEntity : class
        {
            public int PageSize { get; set; }
            public GenericSortOptions<TEntity> SortOptions { get; set; }
            public IQueryFilterOptions<TEntity> FilterOptions { get; set; }
        }
    }
}
