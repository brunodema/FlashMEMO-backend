using Data.Tools.Sorting;
using Data.Tools.Filtering;
using Tests.Integration.Interfaces;

namespace Tests.Integration.Implementation
{
    public static class TestMessages
    {
        /// <summary>
        /// {0} Number of the test.
        /// </summary>
        public const string TestSuccessful = "Test #{0} run successfully.";
        /// <summary>
        /// {0} Number of the test.
        /// {1} Exception message.
        /// </summary>
        public const string TestFailed = "Test #{0} has failed. Exception: {1}";
    }
    public class ValidationErrorsWhenCreatingData<TEntity> : IExpectedValidationErrorsForEntity<TEntity>
    {
        public TEntity Entity { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
    public class GetsSpecifiedNumberOfRecordsPerPageData<TEntity> : IPageData<TEntity>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public class SearchParameters<TEntity> : ISearchParameters<TEntity>
        where TEntity : class
    {
        public int PageSize { get; set; }
        public GenericSortOptions<TEntity> SortOptions { get; set; }
        public IQueryFilterOptions<TEntity> FilterOptions { get; set; }

    }
}
