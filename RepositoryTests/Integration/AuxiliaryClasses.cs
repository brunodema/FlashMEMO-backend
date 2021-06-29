using Data.Tools.Implementations;
using Data.Tools.Interfaces;
using Tests.Integration.Interfaces;

namespace Tests.Integration.AuxiliaryClasses
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
    public class ValidationErrorsWhenCreatingData<TEntiy> : IValidationErrorsWhenCreatingData<TEntiy>
    {
        public TEntiy Entiy { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
    public class GetsSpecifiedNumberOfRecordsPerPageData<TEntiy> : IGetsSpecifiedNumberOfRecordsPerPageData<TEntiy>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    public class ShouldSearchRecordsAppropriately<TEntiy> : IShouldSearchRecordsAppropriately<TEntiy>
        where TEntiy : class
    {
        public int PageSize { get; set; }
        public string ColumnToSort { get; set; }
        public SortType SortType { get; set; }
        public IQueryFilterOptions<TEntiy> FilterOptions { get; set; }
    }
}
