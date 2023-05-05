using System;
using UnityEngine;


namespace Game.Scripts.Logic
{
    public sealed class InterpolatedGameObjectView : MonoBehaviour
    {
        [SerializeField] private GameObjectView _view;
        [SerializeField] private bool _disable;
        [SerializeField] private bool _enableDebug = false;
        private bool _setup;

        private float _tickTime;
        private float _tickDelta;

        private Vector2 _prevPos2;
        private Vector2 _prevPos1;
        private Vector2 _prevPos0;

        public GameObjectView GameObjectView => _view;

        public void SetPosition(Vector2 position, float tickTitme, float tickDelta)
        {
            _tickTime = tickTitme;
            _tickDelta = tickDelta;

            _prevPos2 = _prevPos1;
            _prevPos1 = _prevPos0;
            _prevPos0 = position;

            _setup = true;
        }

        private void Start()
        {
            _prevPos2 = _prevPos1 = _prevPos0 = _view.Position;
        }

        private void LateUpdate()
        {
            UpdatePosition(Time.realtimeSinceStartup);
        }

        private void UpdatePosition(float time)
        {
            Vector2 start = _view.Position;
            _view.Position = _disable ? _prevPos0 : GetInterpolatedPosition(time);
            
#if UNITY_EDITOR
            if (_enableDebug)
            {
                Vector2 end = _view.Position;
                float speed = (end - start).magnitude;
                var color = _setup ? Color.magenta : Color.cyan;
                Debug.DrawRay(end, Vector3.up * speed * 10f, color, 1f);
            }
#endif
            _setup = false;
        }

        public Vector2 GetInterpolatedPosition(float currentTime)
        {
            float t = (currentTime - _tickTime) / _tickDelta;
            return Vector2.Lerp(_prevPos2, _prevPos1, t);
        }

#if UNITY_EDITOR

        private void Reset()
        {
            _view = GetComponent<GameObjectView>();
        }

#endif

    }
}