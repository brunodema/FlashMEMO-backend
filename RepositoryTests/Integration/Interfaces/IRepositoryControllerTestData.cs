using Data.Repository.Interfaces;
using System.Collections.Generic;

namespace Tests.Integration.Interfaces
{
    public interface IRepositoryControllerTestData<TEntity, TKey> 
        where TEntity : class, IDatabaseItem<TKey>
    {
        public IEnumerable<TEntity> CreatesSuccessfullyTestCases { get; }
        public IEnumerable<object[]> DeletesByIdSuccessfullyTestData { get; }
        public IEnumerable<object[]> FailsDeletionIfIdDoesNotExistTestData { get; }
        public IEnumerable<object[]> ReportsValidationErrorsWhenCreatingTestData { get; }
        public IEnumerable<object[]> ReportsValidationErrorsWhenUpdatingTestData { get; }
        public IEnumerable<object[]> UpdatesSuccessfullyTestData { get; }
    }
}
