using System;
using UnityEngine;


namespace Game.Scripts.Logic
{
    public class SmartCamera : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private Camera _camera;
        [SerializeField, Min(1)] private float _baseSize;
        [SerializeField] private float _sizeChangePow = 2f;
        [SerializeField] private float _startResizing = 0.8f;
        [SerializeField] private float _sizeLerpSped = 5f;
        [SerializeField] private float _positionLerpSped = 5f;
        [SerializeField] private Transform[] _targets;

        private float _size;

        private void Start()
        {
            _size = _baseSize;
        }

        private void LateUpdate()
        {
            if (_targets == null)
                return;

            GetBounds(out Vector2 min, out Vector2 max, out Vector2 center);
            _size = GetNewSize(min, max);

            _root.position = Vector3.Lerp(_root.position, center, Time.deltaTime * _positionLerpSped);
            
            if (_camera.orthographicSize != _size)
                _camera.orthographicSize = _size;
        }

        private float GetNewSize(Vector2 min, Vector2 max)
        {
            float farthest = GetFarthestSide(min, max);

            float scale = farthest > _startResizing ? farthest / _startResizing : 1f;
            float targetOrtho = _baseSize * Mathf.Pow(scale, _sizeChangePow);
            return Mathf.Lerp(_size, targetOrtho, Time.deltaTime * _sizeLerpSped);
        }

        private float GetFarthestSide(Vector2 min, Vector2 max)
        {
            Vector2 delta = max - min;
            Vector2 screen = new(Screen.width, Screen.height);
            Vector2 fraction = delta / screen;
            return Mathf.Max(fraction.x, fraction.y);
        }

        private void GetBounds(out Vector2 min, out Vector2 max, out Vector2 center)
        {
            center = Vector2.zero;

            min = Vector2.positiveInfinity;
            max = Vector2.negativeInfinity;

            for (int i = 0; i < _targets.Length; i++)
            {
                Vector2 wpos = _targets[i].position;
                center += wpos;
                
                Vector2 spos = _camera.WorldToScreenPoint(wpos);
                min = Vector2.Min(spos, min);
                max = Vector2.Max(spos, max);
            }

            center /= _targets.Length;
        }

        private void Reset()
        {
            _root = transform;
            _camera = GetComponent<Camera>();
            _baseSize = _camera.orthographicSize;
        }
    }
}