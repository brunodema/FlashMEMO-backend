using Data.Repository.Interfaces;
using System;

namespace Tests.Integration.Interfaces
{
    public interface IRepositoryControllerTests<TEntity> :
        IRepositoryControllerCreateTests<TEntity>,
        IRepositoryControllerUpdateTests<TEntity>,
        IRepositoryControllerDeleteTests<TEntity>,
        IRepositoryControllerGetTests<TEntity>
        where TEntity : class
    {
        public IBaseRepository<TEntity> BaseRepository { get; set; }
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
    public interface IRepositoryControllerDeleteTests<TEntity>
    {
        void DeletesSuccessfully(TEntity entity);
        void DeletesByIdSuccessfully(Guid guid);
        void FailsDeletionIfIdDoesNotExist(Guid guid);
    }
    public interface IRepositoryControllerGetTests<TEntity>
    {
        void GetsAllRecordsSuccessfully(int expectedNumberOfRecords);
        void GetsSpecifiedNumberOfRecordsAtMax(int numberOfRecords);
        void GetsSpecifiedNumberOfPagesAndRecords(int pageSize, int numberOfPages);
    }
}
