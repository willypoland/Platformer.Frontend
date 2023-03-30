using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace Common.Pool
{
    public class IndexPool : IEnumerable<IndexInstance>
    {
        public const int InitialCapacity = 8;

        private int[] _items;
        private int _count = 0;
        private readonly Stack<int> _released;

        public IndexPool(int initialCapacity = InitialCapacity)
        {
            _items = new int[initialCapacity];
            _released = new Stack<int>(initialCapacity);
        }

        public int ActiveCount => _count - _released.Count;

        public int AllCount => _count;

        public int ReleasedCount => _released.Count;

        public int Capacity => _items.Length;

        public IndexInstance Get()
        {
            if (!_released.TryPop(out int idx))
            {
                idx = AddIndex();
            }
            
            ref int gen = ref _items[idx];
            gen = -gen + 1;
            return new IndexInstance(idx, gen);
        }

        public bool Exist(IndexInstance instance)
        {
            return instance.Index >= 0 &&
                   instance.Index < _count &&
                   instance.IsValid &&
                   _items[instance.Index] == instance.Generation;
        }

        public bool TryRelease(in IndexInstance instance)
        {
            if (Exist(instance))
            {
                UnsafeRelease(instance);
                return true;
            }

            return false;
        }

        public void UnsafeRelease(in IndexInstance instance)
        {
            ref int idx = ref _items[instance.Index];
            idx = SetMinus(idx);
            _released.Push(instance.Index);
        }

        public void ReleaseAll()
        {
            for (int i = 0; i < _count; i++)
            {
                ref int idx = ref _items[i];
                idx = SetMinus(idx);
            }

            _count = 0;
            _released.Clear();
        }

        private int AddIndex()
        {
            while (_count >= _items.Length)
                Resize(_items.Length * 2);

            int index = _count;
            _count++;
            return index;
        }

        public void Resize(int newSize)
        {
            int[] newItems = new int[newSize];

            if (_count > 0)
            {
                int copyCount = Math.Min(_count, newSize);
                Array.Copy(_items, newItems, copyCount);
                _count = copyCount;
            }

            _items = newItems;
        }

        public IEnumerator<IndexInstance> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                var gen = _items[i];
                if (gen > 0)
                    yield return new IndexInstance(i, gen);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString() => $"IndexPool[{ActiveCount}/{AllCount}/{Capacity}]";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int SetMinus(int value)
        {
            return value > 0 ? -value : value;
        }
    }
}