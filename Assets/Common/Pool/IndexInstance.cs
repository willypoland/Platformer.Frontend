namespace Common.Pool
{
    public struct IndexInstance
    {
        public readonly int Index;
        public readonly int Generation;

        internal IndexInstance(int idx, int generation)
        {
            Index = idx;
            Generation = generation;
        }

        public bool IsValid => Generation > 0;

        public override string ToString() => IsValid ? $"<{Index}:{Generation}>" : "<INVALID>";
    }

}