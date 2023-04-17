using System;
using Game.Code.Data;
using Game.Code.Data.Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Game.Code.UI
{
    public class SettingsView : MonoBehaviour, ISettingsView
    {
        [SerializeField] private Slider _projectionSize;
        [SerializeField] private Slider _framerate;
        [SerializeField] private Toggle _vsync;
        [SerializeField] private Toggle _lerp;
        [SerializeField] private Toggle _playerShape;
        [SerializeField] private Toggle _syncDll;
        [SerializeField] private Toggle _asyncDll;
        [SerializeField] private Toggle _ggpoDll;

        private readonly UnityEvent<BackendType> _backendTypeChanged = new();
        
        public UnityEvent<float> ProjectionSizeChanged => _projectionSize.onValueChanged;
        public UnityEvent<float> FramerateChanged => _framerate.onValueChanged;
        public UnityEvent<bool> VsyncChanged => _vsync.onValueChanged;
        public UnityEvent<bool> LerpChanged => _lerp.onValueChanged;
        public UnityEvent<bool> PlayerShapeChanged => _playerShape.onValueChanged;
        public UnityEvent<BackendType> BackendTypeChanged => _backendTypeChanged;

        private void Awake()
        {
            _syncDll.onValueChanged.AddListener(x => SetIfTrue(x, BackendType.Sync));
            _asyncDll.onValueChanged.AddListener(x => SetIfTrue(x, BackendType.Async));
            _ggpoDll.onValueChanged.AddListener(x => SetIfTrue(x, BackendType.GGPO));
        }

        public void SetValuesWithoutNotify(GameSettings settings)
        {
            _projectionSize.SetValueWithoutNotify(settings.Graphics.ProjectionSize);
            _framerate.SetValueWithoutNotify(settings.Graphics.TargetFramerate);
            _vsync.SetIsOnWithoutNotify(settings.Graphics.Vsync);
            
            _lerp.SetIsOnWithoutNotify(settings.Other.EnablePositionLerp);
            _playerShape.SetIsOnWithoutNotify(settings.Other.ShowPlayerShape);
            SetBackendTypeTyggle(settings.Other.BackendType);
        }

        private void SetIfTrue(bool value, BackendType type)
        {
            if (value) SetBackendTypeTyggle(type);
        }

        private void SetBackendTypeTyggle(BackendType type)
        {
            _syncDll.SetIsOnWithoutNotify(type == BackendType.Sync);
            _asyncDll.SetIsOnWithoutNotify(type == BackendType.Async);
            _ggpoDll.SetIsOnWithoutNotify(type == BackendType.GGPO);
            _backendTypeChanged.Invoke(type);
        }
    }


}