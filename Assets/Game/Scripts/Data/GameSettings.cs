using UnityEngine;


namespace Game.Scripts.Data
{
    [System.Serializable]
    public sealed class GameSettings
    {
        // graphics
        public float ProjectionSize = 100f;
        public int TargetFramerate = 60;
        public bool Vsync = true;

        // other
        public bool EnablePositionLerp = true;
        public float LerpSpeed = 30f;
        public bool ShowPlayerShape = false;
        public BackendType BackendType = BackendType.Sync;
        
        // keymaps
        public KeyCode InputRight = KeyCode.D;
        public KeyCode Inputleft = KeyCode.A;
        public KeyCode InputJump = KeyCode.W;
        public KeyCode InputDown = KeyCode.S;
        public KeyCode InputAttack = KeyCode.Mouse0;
        public KeyCode InputConnectionWindow = KeyCode.Tab;
        public KeyCode InputSettingsWindow = KeyCode.Tab;
        public KeyCode InputExit = KeyCode.Escape;
        public KeyCode InputDebugInfo = KeyCode.BackQuote;
    }
    
    
}