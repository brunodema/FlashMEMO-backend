using Tests.Integration.Interfaces;

namespace Tests.Integration.AuxiliaryClasses
{
    public class ValidationErrorsWhenCreatingData<TEntiy> : IValidationErrorsWhenCreatingData<TEntiy>
    {
        public TEntiy Entiy { get; set; }
        public string[] Errors { get; set; }
    }
    public class GetsSpecifiedNumberOfRecordsPerPageData<TEntiy> : IGetsSpecifiedNumberOfRecordsPerPageData<TEntiy>
    {
        public int pageSize { get; set; }
        public int? pageNumber { get; set; }
    }
}
