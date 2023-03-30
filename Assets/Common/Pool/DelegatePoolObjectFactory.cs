using System;


namespace Common.Pool
{
    public sealed class DelegatePoolObjectFactory<T> : IPoolObjectFactory<T> where T : class
    {
        private Func<T> _createFunc;

        public DelegatePoolObjectFactory(Func<T> createFunc, Action<T> getAction = null, Action<T> releaseAction = null,
                                     Action<T> disposeAction = null)
        {
            CreateFunc = createFunc;
            GetAction = getAction;
            ReleaseAction = releaseAction;
            DisposeAction = disposeAction;
        }

        public Func<T> CreateFunc
        {
            get => _createFunc;
            set => _createFunc = value ?? _createFunc;
        }

        public Action<T> GetAction { get; set; }

        public Action<T> ReleaseAction { get; set; }

        public Action<T> DisposeAction { get; set; }

        public T Create() => CreateFunc.Invoke();

        public void ActionOnGet(T obj) => GetAction?.Invoke(obj);

        public void ActionOnRelease(T obj) => ReleaseAction?.Invoke(obj);

        public void ActionOnDispose(T obj) => DisposeAction?.Invoke(obj);
    }
}