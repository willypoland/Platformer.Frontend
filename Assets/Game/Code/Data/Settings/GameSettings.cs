namespace Game.Code.Data.Settings
{
    [System.Serializable]
    public sealed class GameSettings
    {
        public GraphicsSettings Graphics = new();
        public OtherSettings Other = new();
        public KeyMapSettings KeyMap = new();
        public ConnectionArguments InitialConnection = new();
    }
    
    
}