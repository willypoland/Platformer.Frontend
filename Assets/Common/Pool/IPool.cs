namespace Common.Pool
{
    public interface IPool<T>
    {
        PooledItem<T> Get();

        bool IsValid(in PooledItem<T> item);

        bool TryRelease(in PooledItem<T> item);

        void ReleaseAll();

        void Clear();
    }
}