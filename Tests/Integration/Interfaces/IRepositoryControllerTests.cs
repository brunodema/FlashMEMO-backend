namespace Tests.Integration.Interfaces
{
    public interface IRepositoryControllerTests<TEntity, TKey> :
        IRepositoryControllerCreateTests<TEntity>,
        IRepositoryControllerUpdateTests<TEntity>,
        IRepositoryControllerDeleteTests<TEntity, TKey>,
        IRepositoryControllerGetTests<TEntity>,
        IRepositoryControllerListTests<TEntity>
        where TEntity : class
    {
        public string BaseEndpoint { get; set; }
        public string CreateEndpoint { get; set; }
        public string UpdateEndpoint { get; set; }
        public string GetEndpoint { get; set; }
        public string ListEndpoint { get; set; }
        public string DeleteEndpoint { get; set; }
        public string SearchEndpoint { get; set; }
    }
    public interface IRepositoryControllerCreateTests<TEntity>
    {
        void CreatesSuccessfully();
        void ReportsValidationErrorsWhenCreating();
    }
    public interface IRepositoryControllerUpdateTests<TEntity>
    {
        void UpdatesSuccessfully();
        void ReportsValidationErrorsWhenUpdating();
    }
    public interface IRepositoryControllerDeleteTests<TEntity, TKey>
    {
        void DeletesByIdSuccessfully();
        void FailsDeletionIfIdDoesNotExist();
    }
    public interface IRepositoryControllerGetTests<TEntity>
    {
        void GetsSpecifiedNumberOfRecordsPerPage();
    }
    public interface IRepositoryControllerListTests<TEntity>
    {
        void ListsAllRecordsSuccessfully();
        void ShouldSearchRecordsAppropriately();
    }
}
