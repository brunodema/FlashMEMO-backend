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
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
    }

    public class ShouldSortRecordsAppropriately<TEntiy> : IShouldSortRecordsAppropriately<TEntiy>
    {
        public int pageSize { get; set; }
        public string columnToSort { get; set; }
    }
    public class ShoulFilterRecordsAppropriately<TEntiy> : IShoulFilterRecordsAppropriately<TEntiy>
    {
        public int pageSize { get; set; }
        public string searchString { get; set; }
    }
}
