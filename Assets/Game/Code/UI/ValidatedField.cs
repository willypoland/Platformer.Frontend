namespace Game.Code.UI
{
    public struct ValidatedField<T>
    {
        public T Field;
        public bool IsValid;
        public string Message;
    }
}