using Tests.Integration.Interfaces;

namespace Tests.Integration
{
    public class ValidationErrorsWhenCreatingData<TEntiy> : IValidationErrorsWhenCreatingData<TEntiy>
    {
        public TEntiy Entiy { get; set; }
        public string[] Errors { get; set; }
    }
}
