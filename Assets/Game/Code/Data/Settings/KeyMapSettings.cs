using UnityEngine;


namespace Game.Code.Data.Settings
{
    [System.Serializable]
    public class KeyMapSettings
    {
        public KeyCode Right = KeyCode.D;
        public KeyCode left = KeyCode.A;
        public KeyCode Jump = KeyCode.W;
        public KeyCode Down = KeyCode.S;
        public KeyCode Attack = KeyCode.Mouse0;
        public KeyCode ConnectionWindow = KeyCode.Tab;
        public KeyCode SettingsWindow = KeyCode.Tab;
        public KeyCode Exit = KeyCode.Escape;
        public KeyCode DebugInfo = KeyCode.BackQuote;
    }
}