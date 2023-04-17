using CsUtility.Pool;
using Game.Code.Data;
using Game.Code.Logic;
using UnityEngine;


namespace Game.Code.Scene
{
    public class SceneViewObjectFactory : MonoBehaviour, IViewObjectFactory,
                                          IObjectPoolFactory<ShapeView>,
                                          IObjectPoolFactory<PlayerView>
    {
        [SerializeField] private Transform _shapeRoot;
        [SerializeField] private Transform _playerRoot;
        [SerializeField] private ShapeView _shapeOrigin;
        [SerializeField] private PlayerView _playerOrigin;
        private ObjectPool<ShapeView> _shapePool;
        private ObjectPool<PlayerView> _playerPool;
        private GameConfig _config;

        [VContainer.Inject]
        public void InjectParameters(GameConfig config)
        {
            _config = config;
        }

        private void Start()
        {
            _shapePool = new ObjectPool<ShapeView>(this);
            _shapePool.PrepareInstances(10);
            
            _playerPool = new ObjectPool<PlayerView>(this);
            _playerPool.PrepareInstances(10);
        }

        public void Clear()
        {
            _shapePool.ReleaseAll();
            _playerPool.ReleaseAll();
        }

        public void ShowShape(Api.IGameObject obj, Color color)
        {
            var shape = _shapePool.Get();
            shape.Item.Color = color;
            shape.Item.Set(obj.Position, obj.Size);
        }

        public void ShowPlayer(Api.IPlayer player)
        {
            var view = _playerPool.Get();
            view.Item.Set(player, _config);
        }

        #region Shape pool
        
        public ShapeView Create()
        {
            var newItem = Instantiate(_shapeOrigin, _shapeRoot);
            newItem.gameObject.SetActive(false);
            return newItem;
        }

        public void ActionOnGet(ShapeView obj) => obj.gameObject.SetActive(true);

        public void ActionOnRelease(ShapeView obj) => obj.gameObject.SetActive(false);

        public void ActionOnDispose(ShapeView obj) => Destroy(obj);
        
        #endregion

        #region Playet view pool

        PlayerView IObjectPoolFactory<PlayerView>.Create()
        {
            var newItem = Instantiate(_playerOrigin, _playerRoot);
            newItem.gameObject.SetActive(false);
            return newItem;
        }

        public void ActionOnGet(PlayerView obj) => obj.gameObject.SetActive(true);

        public void ActionOnRelease(PlayerView obj) => obj.gameObject.SetActive(false);

        public void ActionOnDispose(PlayerView obj) => Destroy(obj);

        #endregion
    }
}