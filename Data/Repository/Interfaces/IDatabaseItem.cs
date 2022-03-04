using Xunit.Abstractions;

namespace Data.Repository.Interfaces
{
    public interface IDatabaseItem<TKey>
    {
        public TKey DbId { get; set; }
    }
}
