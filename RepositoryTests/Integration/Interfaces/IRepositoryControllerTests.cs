using Data.Repository.Interfaces;
using System;

namespace Tests.Integration.Interfaces
{
    public interface IRepositoryControllerTests<TEntity, TKey> :
        IRepositoryControllerCreateTests<TEntity>,
        IRepositoryControllerUpdateTests<TEntity>,
        IRepositoryControllerDeleteTests<TEntity, TKey>,
        IRepositoryControllerGetTests<TEntity>
        where TEntity : class
    {
        public IBaseRepository<TEntity, TKey> BaseRepository { get; set; }
        public string BaseEndpoint { get; set; }
        public string CreateEndpoint { get; set; }
        public string UpdateEndpoint { get; set; }
        public string GetEndpoint { get; set; }
        public string DeleteEndpoint { get; set; }
    }
    public interface IRepositoryControllerCreateTests<TEntity>
    {
        void CreatesSuccessfully(TEntity entity);
        void ReportsValidationErrorsWhenCreating(TEntity entity, string[] expectedErrors);
    }
    public interface IRepositoryControllerUpdateTests<TEntity>
    {
        void UpdatesSuccessfully(TEntity entity);
        void ReportsValidationErrorsWhenUpdating(TEntity entity, string[] expectedErrors);
    }
    public interface IRepositoryControllerDeleteTests<TEntity, TKey>
    {
        void DeletesByIdSuccessfully(TKey id);
        void FailsDeletionIfIdDoesNotExist(TKey id);
    }
    public interface IRepositoryControllerGetTests<TEntity>
    {
        void GetsAllRecordsSuccessfully(int expectedNumberOfRecords);
        void GetsSpecifiedNumberOfRecordsAtMax(int numberOfRecords);
        void GetsSpecifiedNumberOfPagesAndRecords(int pageSize, int numberOfPages);
    }
}
