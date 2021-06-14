using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace Tests.Integration.Interfaces
{
    public interface IRepositoryControllerTestData<TEntity, TKey> 
        where TEntity : class, IDatabaseItem<TKey>
    {
        public List<TEntity> CreatesSuccessfullyTestCases { get; }
        public IEnumerable<Guid> DeletesByIdSuccessfullyTestData { get; }
        public IEnumerable<Guid> FailsDeletionIfIdDoesNotExistTestData { get; }
        public IEnumerable<ValidationErrorsWhenCreatingData<TEntity>> ReportsValidationErrorsWhenCreatingTestData { get; }
        public IEnumerable<ValidationErrorsWhenCreatingData<TEntity>> ReportsValidationErrorsWhenUpdatingTestData { get; }
        public IEnumerable<TEntity> UpdatesSuccessfullyTestData { get; }
    }
}
