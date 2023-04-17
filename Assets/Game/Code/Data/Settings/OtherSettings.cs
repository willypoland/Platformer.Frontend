namespace Game.Code.Data.Settings
{
    [System.Serializable]
    public sealed class OtherSettings
    {
        public bool EnablePositionLerp = true;
        public bool ShowPlayerShape = false;
        public BackendType BackendType = BackendType.Sync;
    }
}