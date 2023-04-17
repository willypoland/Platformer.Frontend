using System;
using Game.Code.Data;
using Game.Code.Data.Settings;
using Game.Code.Infrastructure.StateMachine;
using Game.Code.Services;
using Game.Code.UI;
using UnityEngine;
using VContainer.Unity;


namespace Game.Code.Logic
{
    public class GameSettingsController : ILateTickable, IStartable, IDisposable
    {
        private readonly GameSettingsService _settingsService;
        private readonly IInputService _inputService;
        private readonly IStateController _stateController;
        private readonly ISettingsView _settingsView;
        private readonly Camera _camera;

        private GameSettings _settings;

        public GameSettingsController(
            GameSettingsService settingsService,
            IInputService inputService, 
            IStateController stateController,
            ISettingsView settingsView,
            Camera camera)
        {
            _settingsService = settingsService;
            _inputService = inputService;
            _stateController = stateController;
            _settingsView = settingsView;
            _camera = camera;
        }

        public void Start()
        {
            _settings = _settingsService.Read();
            _settingsService.Changed += SettingsServiceOnChanged;
            _settingsView.FramerateChanged.AddListener(OnFramerateChanged);
            _settingsView.LerpChanged.AddListener(OnLerpChanged);
            _settingsView.VsyncChanged.AddListener(OnVSyncChanged);
            _settingsView.BackendTypeChanged.AddListener(OnBackendTypeChanged);
            _settingsView.ProjectionSizeChanged.AddListener(OnProjectionSizeChanged);
            _settingsView.PlayerShapeChanged.AddListener(OnPlayerShapeChanged);
        }

        private void OnVSyncChanged(bool arg0)
        {
            _settings.Graphics.Vsync = arg0;
        }

        private void OnProjectionSizeChanged(float arg0)
        {
            var size = Mathf.Clamp(arg0, Constants.MinProjectionSize, Constants.MaxProjectionSize);
            _settings.Graphics.ProjectionSize = size;
            Save();
        }

        private void OnFramerateChanged(float arg0)
        {
            int input = Mathf.RoundToInt(arg0);
            int framerate = Mathf.Clamp(input, Constants.MinTargetFramerate, Constants.MaxTargetFramerate);
            _settings.Graphics.TargetFramerate = framerate;
            Save();
        }

        private void OnPlayerShapeChanged(bool arg0)
        {
            _settings.Other.ShowPlayerShape = arg0;
            Save();
        }

        private void OnBackendTypeChanged(BackendType arg0)
        {
            _settings.Other.BackendType = arg0;
            Save();
        }

        private void OnLerpChanged(bool arg0)
        {
            _settings.Other.EnablePositionLerp = arg0;
            Save();
        }

        public void Dispose()
        {
            _settingsService.Changed -= SettingsServiceOnChanged;
        }

        private void SettingsServiceOnChanged(GameSettings settings)
        {
            _settings = settings;
            _settingsView.SetValuesWithoutNotify(settings);

            QualitySettings.vSyncCount = settings.Graphics.Vsync ? 1 : 0;
            Application.targetFrameRate = settings.Graphics.TargetFramerate;
            _camera.orthographicSize = settings.Graphics.ProjectionSize;
        }

        public void LateTick()
        {
            if (_stateController.IsActive<GameplayState>())
            {
                ReadInput();
            }
        }

        private void ReadInput()
        {
            if (_inputService.Exit)
                Application.Quit();

            if (_inputService.Wheel != 0)
            {
                float delta = _inputService.Wheel * Time.unscaledTime;
                float size = _settings.Graphics.ProjectionSize;
                OnProjectionSizeChanged(size + delta);
            }
        }

        private void Save()
        {
            _settingsService.Write(_settings);
        }

    }
}