using System;
using CsUtility.Extensions;
using Game.Scripts.Common;
using Game.Scripts.Data;
using Game.Scripts.Infrastructure;
using UnityEngine;


namespace Game.Scripts.Logic
{

    public class PlatformMarker : MonoBehaviour
    {
        [SerializeField] private PlatformType _type;
        [SerializeField] private Vector2 _offset = Vector2.zero;
        [SerializeField] private Vector2 _size = Vector2.one;

        public PlatformType Type => _type;

        private void OnValidate()
        {
            _size.x = Mathf.Max(0, _size.x);
            _size.y = Mathf.Max(0, _size.y);
        }

        private void OnDrawGizmosSelected()
        {
            var config = Resources.Load<GameConfig>(AssetPath.GameConfig);

            var max = transform.TransformPoint(_size / 2f);
            var min = transform.TransformPoint(_size / -2f);

            var maxSize = (config.MaxSceneSize / 2f);
            var minSize = -maxSize;

            Gizmos.color = min.x >= minSize.x && min.y >= minSize.y && max.x <= maxSize.x && max.y <= maxSize.y
                ? config.PlatfromGizmoColor
                : config.PlatfromMarkerInvalidColor;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(_offset, _size);
        }

        public Rect GetRect()
        {
            Vector2 min = transform.TransformPoint(_size / -2f);
            return new Rect(min.x, min.y, _size.x, _size.y);
        }
    }
}