using Game.Scripts.Common;
using Game.Scripts.Data;
using Game.Scripts.Infrastructure;
using UnityEngine;


namespace Game.Scripts.Logic
{
    public class TransformMarker : MonoBehaviour
    {
        [SerializeField] private Vector2 _offset = Vector2.zero;
        [SerializeField] private Vector2 _size = Vector2.one;
        
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
        
        public Rect ToViewRect()
        {
            Vector2 size = transform.localScale * _size;
            Vector2 pos = (Vector2)transform.position + (size / -2f + _offset);
            return new Rect(pos, size);
        }

        public void SetViewRect(Rect rect)
        {
            transform.position = rect.center;
            transform.localScale = rect.size;
        }
    }


}