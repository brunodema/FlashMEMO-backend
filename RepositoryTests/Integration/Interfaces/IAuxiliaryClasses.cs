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
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
    }

    public interface IShouldSortRecordsAppropriately<TEntiy>
    {
        public int pageSize { get; set; }
        public string columnToSort{ get; set; }
    }

    public interface IShoulFilterRecordsAppropriately<TEntiy>
    {
        public int pageSize { get; set; }
        public string searchString { get; set; }
    }
}