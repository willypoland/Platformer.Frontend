using Api;
using UnityEngine;


namespace Game.Scripts.Logic
{
    [RequireComponent(typeof(TransformMarker))]
    public class PlatformMarker : MonoBehaviour
    {
        [SerializeField] private PlatformType _type;
        private TransformMarker _transformMarker;

        public PlatformType Type => _type;
        public Rect Rect => _transformMarker.ToViewRect();

        private void Awake()
        {
            _transformMarker = GetComponent<TransformMarker>();
        }
    }
}