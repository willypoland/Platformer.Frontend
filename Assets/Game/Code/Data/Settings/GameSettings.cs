namespace Game.Code.Data.Settings
{
    [System.Serializable]
    public sealed class GameSettings
    {
        public readonly GraphicsSettings Graphics = new();
        public readonly OtherSettings Other = new();
        public readonly KeyMapSettings KeyMap = new();
        public readonly ConnectionArguments InitialConnection = new();
    }
    
    
}