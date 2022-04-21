using Xunit.Abstractions;

namespace Data.Repository.Interfaces
{
    public interface IDatabaseItem<TKey>
    {
        TKey DbId { get; set; }
    }
}
