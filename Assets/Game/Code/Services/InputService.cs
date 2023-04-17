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
            _settings ??= _settingsService.Read().KeyMap;
            
            var map = new InputMap()
            {
                LeftPressed = Input.GetKey(_settings.left),
                RightPressed = Input.GetKey(_settings.Right),
                UpPressed = Input.GetKey(_settings.Jump),
                DownPressed = Input.GetKey(_settings.Down),
                LeftMouseClicked = Input.GetKey(_settings.Attack),
            };

            return map;
        }
    }
}