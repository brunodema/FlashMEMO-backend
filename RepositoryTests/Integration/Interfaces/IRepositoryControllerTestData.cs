using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using Tests.Integration.AuxiliaryClasses;

// Attention: always use different existing objects when necessary (i.e., different GUIDs). Unfortunately, since the tests are run concurrently, sometimes that causes issues, making tests fail when they shouldn't.

namespace Tests.Integration.Interfaces
{
    public interface IRepositoryControllerTestData<TEntity, TKey> 
        where TEntity : class, IDatabaseItem<TKey>
    {
        public List<TEntity> CreatesSuccessfullyTestCases { get; }
        public IEnumerable<Guid> DeletesByIdSuccessfullyTestData { get; }
        public IEnumerable<Guid> FailsDeletionIfIdDoesNotExistTestData { get; }
        public IEnumerable<int> ListsAllRecordsSuccessfully { get; }
        public IEnumerable<GetsSpecifiedNumberOfRecordsPerPageData<TEntity>> GetsSpecifiedNumberOfRecordsPerPage { get; }
        public IEnumerable<IValidationErrorsWhenCreatingData<TEntity>> ReportsValidationErrorsWhenCreatingTestData { get; }
        public IEnumerable<IValidationErrorsWhenCreatingData<TEntity>> ReportsValidationErrorsWhenUpdatingTestData { get; }
        public IEnumerable<TEntity> UpdatesSuccessfullyTestData { get; }
        public IEnumerable<IShouldSortRecordsAppropriately<TEntity>> ShouldSortRecordsAppropriatelyTestData { get; }
        public IEnumerable<IShoulFilterRecordsAppropriately<TEntity>> ShoulFilterRecordsAppropriatelyTestData { get; }
    }
}
