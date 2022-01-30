using Data.Repository.Interfaces;

namespace RepositoryTests
{
    public interface IBaseRepositoryTests<TEntity, TKey> where TEntity : IDatabaseItem<TKey>
    {
        public void CreateAsync_AssertThatItGetsProperlyCreated();
        public void UpdateAsync_AssertThatItGetsProperlyUpdated();
        public void RemoveAsync_AssertThatItGetsProperlyRemoved();
        public void GetByIdAsync_AssertThatItGetsProperlyRemoved();
    }
}
