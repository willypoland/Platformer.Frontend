using System;
using System.Collections.Generic;
using UnityEngine;


namespace Common.Pool
{
    public class ObjectPool<T> : IPool<T> where T : class
    {
        private readonly IPoolObjectFactory<T> _factory;
        private readonly IndexPool _indexPool;
        private T[] _objects;

        public ObjectPool(IPoolObjectFactory<T> factory, int capacity = 8)
        {
            _factory = factory;
            _indexPool = new IndexPool(capacity);
        }

        public void PrepareInstances(int preparedInstance)
        {
            if (_objects != null && preparedInstance < _objects.Length)
                return;
            
            _indexPool.Resize(preparedInstance);
            ResizeObjectsBuffer(_indexPool.Capacity);
        }

        public PooledItem<T> Get()
        {
            var index = GetIndexAndResize();
            var item = new PooledItem<T>(index, _objects[index.Index]);
            _factory.ActionOnGet(item.Item);
            return item;
        }

        public bool IsValid(in PooledItem<T> item)
        {
            return _indexPool.Exist(item.Idx);
        }

        public bool TryRelease(in PooledItem<T> item)
        {
            if (!IsValid(item))
                return false;
            
            _factory.ActionOnRelease(item.Item);
            _indexPool.UnsafeRelease(item.Idx);

            return true;
        }

        public void ReleaseAll()
        {
            foreach (var idx in _indexPool)
            {
                var obj = _objects[idx.Index];
                _factory.ActionOnRelease(obj);
            }
            _indexPool.ReleaseAll();
        }

        public void Clear()
        {
            _indexPool.ReleaseAll();
            for (int i = 0; i < _objects.Length; i++)
            {
                _factory.ActionOnDispose(_objects[i]);
                _objects[i] = null;
            }
        }

        private IndexInstance GetIndexAndResize()
        {
            var ii = _indexPool.Get();

            if (_objects == null || _objects.Length < _indexPool.Capacity)
            {
                ResizeObjectsBuffer(_indexPool.Capacity);
                Debug.Log("Resized");
            }

            return ii;
        }

        private void ResizeObjectsBuffer(int newSize)
        {
            int prevLen;
            if (_objects == null)
            {
                prevLen = 0;
                _objects = new T[newSize];
            }
            else
            {
                prevLen = _objects.Length;
                Array.Resize(ref _objects, newSize);
            }

            for (int i = prevLen; i < newSize; i++)
                _objects[i] = _factory.Create();
        }
    }
}