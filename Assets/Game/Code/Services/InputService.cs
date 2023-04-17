using Api;
using Game.Code.Data.Settings;
using Game.Code.Services;
using UnityEngine;


namespace Game.Code.Logic
{
    public class InputService : IInputService
    {
        private readonly GameSettingsService _settingsService;
        private KeyMapSettings _settings;
        
        public InputService(GameSettingsService settingsService)
        {
            _settingsService = settingsService;
            settingsService.Changed += x => _settings = x.KeyMap;
        }

        public InputMap ReadInputMap()
        {
            var keymap = Get();
            
            var map = new InputMap()
            {
                LeftPressed = Input.GetKey(keymap.left),
                RightPressed = Input.GetKey(keymap.Right),
                UpPressed = Input.GetKey(keymap.Jump),
                DownPressed = Input.GetKey(keymap.Down),
                LeftMouseClicked = Input.GetKey(keymap.Attack),
            };

            return map;
        }

        public bool Exit => Input.GetKeyDown(Get().Exit);

        public float Wheel => Input.mouseScrollDelta.y;

        private KeyMapSettings Get()
        {
            return _settings ??= _settingsService.Read().KeyMap;
        }
    }
}