namespace Data.Repository.Interfaces
{
    public interface IDatabaseItem<TKey>
    {
        public TKey GetId();
    }
}
