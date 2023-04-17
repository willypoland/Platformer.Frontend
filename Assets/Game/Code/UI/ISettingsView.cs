using Game.Code.Data.Settings;
using UnityEngine.Events;


namespace Game.Code.UI
{
    public interface ISettingsView
    {
        void SetValuesWithoutNotify(GameSettings settings);
        
        UnityEvent<float> ProjectionSizeChanged { get; }
        UnityEvent<float> FramerateChanged { get; }
        UnityEvent<bool> VsyncChanged { get; }
        UnityEvent<bool> LerpChanged { get; }
        UnityEvent<bool> PlayerShapeChanged { get; }
        UnityEvent<BackendType> BackendTypeChanged { get; }
    }
}