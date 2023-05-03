using System;
using UnityEngine;


namespace Game.Scripts.Logic
{
    public sealed class InterpolatedGameObjectView : MonoBehaviour
    {
        [SerializeField] private GameObjectView _view;
        [SerializeField] private bool _disable;
        private bool _setup;

        private float _tickTime;
        private float _tickDelta;
        private Vector2 _prevPosition;
        private Vector2 _lastPosition;

        public GameObjectView GameObjectView => _view;

        public void SetPosition(Vector2 position, float tickTitme, float tickDelta)
        {
            _tickTime = tickTitme;
            _tickDelta = tickDelta;
            _prevPosition = _lastPosition;
            _lastPosition = position;
            _setup = true;
        }

        private void Start()
        {
            _prevPosition = _lastPosition = _view.Position;
        }

        private void LateUpdate()
        {
            UpdatePosition(Time.realtimeSinceStartup);
        }

        private void UpdatePosition(float time)
        {
            Vector2 start = _view.Position;
            _view.Position = _disable ? _lastPosition : GetInterpolatedPosition(time);
            Vector2 end = _view.Position;
            float speed = (end - start).magnitude;

            var color = _setup ? Color.magenta : Color.cyan;
            Debug.DrawRay(end, Vector3.up * speed * 10f, color, 1f);
            _setup = false;
        }

        public Vector2 GetInterpolatedPosition(float currentTime)
        {
            float t = (currentTime - _tickTime) / _tickDelta;
            return Vector2.Lerp(_prevPosition, _lastPosition, t);
        }

        #region EDITOR'S SHIT

        private void Reset()
        {
            _view = GetComponent<GameObjectView>();
        }

        #endregion

    }
}