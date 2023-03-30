namespace Common.Pool
{
    public interface IPoolObjectFactory<T> where T : class
    {
        T Create();

        void ActionOnGet(T obj);

        void ActionOnRelease(T obj);

        void ActionOnDispose(T obj);
    }
}