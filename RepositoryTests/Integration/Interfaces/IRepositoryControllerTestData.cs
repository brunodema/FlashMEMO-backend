using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using Tests.Integration.AuxiliaryClasses;

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
    }
}
