using Xunit.Abstractions;

namespace Data.Repository.Interfaces
{
    public interface IDatabaseItem<TKey> : IXunitSerializable
    {
        public TKey DbId { get; set; }
    }
}
