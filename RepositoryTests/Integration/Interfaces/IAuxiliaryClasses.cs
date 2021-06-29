using Data.Tools.Implementations;
using Data.Tools.Interfaces;

namespace Tests.Integration.Interfaces
{
    public interface IValidationErrorsWhenCreatingData<TEntiy>
    {
        public TEntiy Entiy { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
    public interface IGetsSpecifiedNumberOfRecordsPerPageData<TEntiy>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public interface IShouldSearchRecordsAppropriately<TEntiy>
        where TEntiy : class
    {
        public int PageSize { get; set; }
        public string ColumnToSort { get; set; }
        SortType SortType { get; set; }
        IQueryFilterOptions<TEntiy> FilterOptions { get; set; }
    }
}