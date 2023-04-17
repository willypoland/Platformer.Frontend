namespace Game.Code.Data.Settings
{
    [System.Serializable]
    public sealed class GraphicsSettings
    {
        public float ProjectionSize = 100f;
        public int TargetFramerate = 60;
        public bool Vsync = true;
    }
}