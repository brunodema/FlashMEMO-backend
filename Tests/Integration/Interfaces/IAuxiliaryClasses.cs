using Data.Tools.Sorting;
using Data.Tools.Interfaces;

namespace Tests.Integration.Interfaces
{
    public interface IExpectedValidationErrorsForEntity<TEntiy>
    {
        public TEntiy Entiy { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
    public interface IPageData<TEntiy>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public interface ISearchParameters<TEntiy>
        where TEntiy : class
    {
        public int PageSize { get; set; }
        public GenericSortOptions<TEntiy> SortOptions { get; set; }
        public IQueryFilterOptions<TEntiy> FilterOptions { get; set; }
    }
}