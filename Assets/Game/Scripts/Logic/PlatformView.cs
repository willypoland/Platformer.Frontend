using Api;
using UnityEngine;


namespace Game.Scripts.Logic
{
    [RequireComponent(typeof(GameObjectView))]
    public class PlatformView : MonoBehaviour
    {
        [SerializeField] private PlatformType _type;
        private GameObjectView _gameObjectView;

        public PlatformType Type => _type;
        public Rect Rect => _gameObjectView.ToViewRect();

        private void Awake()
        {
            _gameObjectView = GetComponent<GameObjectView>();
        }
    }
}