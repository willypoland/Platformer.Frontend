namespace Common.Pool
{
    public struct PooledItem<T>
    {
        public readonly IndexInstance Idx;
        public readonly T Item;

        internal PooledItem(IndexInstance idx, T item)
        {
            Idx = idx;
            Item = item;
        }

        public override string ToString() => $"{Idx}{Item}";
    }
}