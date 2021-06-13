using System.Collections.Generic;

namespace Tests.Integration.Interfaces
{
    public interface IRepositoryControllerTestData
    {
        public static IEnumerable<object[]> CreatesSuccessfullyTestCases { get; }
        public static IEnumerable<object[]> DeletesByIdSuccessfullyTestData { get; }
        public static IEnumerable<object[]> FailsDeletionIfIdDoesNotExistTestData { get; }
        public static IEnumerable<object[]> ReportsValidationErrorsWhenCreatingTestData { get; }
        public static IEnumerable<object[]> ReportsValidationErrorsWhenUpdatingTestData { get; }
        public static IEnumerable<object[]> UpdatesSuccessfullyTestData { get; }
    }
}
