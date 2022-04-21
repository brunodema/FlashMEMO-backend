using Data.Tools.Sorting;
using Data.Tools.Filtering;

namespace Tests.Integration.Interfaces
{
    public interface IExpectedValidationErrorsForEntity<TEntity>
    {
        public TEntity Entity { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
    public interface IPageData<TEntity>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public interface ISearchParameters<TEntity>
        where TEntity : class
    {
        public int PageSize { get; set; }
        public GenericSortOptions<TEntity> SortOptions { get; set; }
        public IQueryFilterOptions<TEntity> FilterOptions { get; set; }
    }
}