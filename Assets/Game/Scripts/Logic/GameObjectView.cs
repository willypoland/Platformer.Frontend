using System;
using Game.Scripts.Common;
using Game.Scripts.Data;
using Game.Scripts.Infrastructure;
using UnityEngine;


namespace Game.Scripts.Logic
{

    public sealed class GameObjectView : MonoBehaviour
    {
        [SerializeField] private Vector2 _offset = Vector2.zero;
        [SerializeField] private Vector2 _scale = Vector2.one;

        private Vector2 _position;
        private Vector2 _size;
        
        public Vector2 Position
        {
            get => _position;
            set
            {
                transform.position = value + _size * 0.5f;
                _position = value;
            }
        }

        public Vector2 Size
        {
            get => _size;
            set
            {
                transform.localScale = value;
                _size = value;
            }
        }

        private void Awake()
        {
            _size = transform.localScale * _scale;
            _position = (Vector2) transform.position + (_size * -0.5f + _offset);
        }

        public Rect ToViewRect()
        {
            return new Rect(_position, _size);
        }

        public void SetViewRect(Rect rect)
        {
            Position = rect.position;
            Size = rect.size;
        }

        #region EDITOR'S SHIT
        
        private void OnValidate()
        {
            _scale.x = Mathf.Max(0, _scale.x);
            _scale.y = Mathf.Max(0, _scale.y);
        }

        private void OnDrawGizmosSelected()
        {
            var config = Resources.Load<GameConfig>(AssetPath.GameConfig);

            var max = transform.TransformPoint(_scale / 2f);
            var min = transform.TransformPoint(_scale / -2f);

            var maxSize = (config.MaxSceneSize / 2f);
            var minSize = -maxSize;

            Gizmos.color = min.x >= minSize.x && min.y >= minSize.y && max.x <= maxSize.x && max.y <= maxSize.y
                ? config.PlatfromGizmoColor
                : config.PlatfromMarkerInvalidColor;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(_offset, _scale);
        }

        #endregion
    }


}