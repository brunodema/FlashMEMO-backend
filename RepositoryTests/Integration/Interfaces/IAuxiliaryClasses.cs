using Data.Tools.Implementations;

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

    public interface IShouldSortRecordsAppropriately<TEntiy>
    {
        public int PageSize { get; set; }
        public string ColumnToSort { get; set; }
        SortType SortType { get; set; }
    }

    public interface IShoulFilterRecordsAppropriately<TEntiy>
    {
        public int PageSize { get; set; }
        public string SearchString { get; set; }
    }
}