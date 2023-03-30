using System.Collections.Generic;
using Api;
using Common.Pool;
using Ser;
using UnityEngine;


namespace Game
{
    public class TestSceneView : MonoBehaviour
    {
        [SerializeField] private Color _playerColor = Color.white;
        [SerializeField] private Color _attackColor = Color.red;
        [SerializeField] private Color _platformColor = Color.yellow;
        [SerializeField] private ShapeMock _shapePrefab;
        [SerializeField] private Transform _sceneRoot;
        [SerializeField] private InitializeFieldView _initializeFieldView;
        [SerializeField] private InputMapView _inputMapView;

        private PlatformerCore _platformer;
        private bool _connected = false;

        private ShapeMock.Factory _shapeFactroy;
        private ObjectPool<ShapeMock> _shapes;

        private void Start()
        {
            _shapeFactroy = new ShapeMock.Factory(_sceneRoot, _shapePrefab);
            _shapes = new ObjectPool<ShapeMock>(_shapeFactroy);
            _shapes.PrepareInstances(10);
            
            _platformer = new PlatformerCore();
            _initializeFieldView.Connect += Connect;
        }

        private void OnDestroy()
        {
            Debug.Log("Stop game");
            _platformer.StopGame();
            _connected = false;
        }

        private void Update()
        {
            if (_connected)
            {
                InputMap input = HandleInput();
                _inputMapView.Set(input);
                _platformer.UpdateTick(input);
                if (_platformer.GameState != null)
                    UpdateScene(_platformer.GameState);
            }
        }

        private void UpdateScene(GameState gs)
        {
            var cam = Camera.main; 
            _shapes.ReleaseAll();

            foreach (var player in gs.Players)
            {
                var shape = GetShape(_playerColor);
                shape.Set(player.Obj.Position.X, player.Obj.Position.Y, player.Obj.Width, player.Obj.Height);
            }

            foreach (var attack in gs.MeleeAttacks)
            {
                var shape = GetShape(_attackColor);
                shape.Set(attack.Position.X, attack.Position.Y, attack.Width, attack.Height);
            }

            foreach (var platform in gs.Platforms)
            {
                var shape = GetShape(_platformColor);
                shape.Set(platform.Position.X, platform.Position.Y, platform.Width, platform.Height);
            }
        }

        private ShapeMock GetShape(Color color)
        {
            var shape = _shapes.Get();
            shape.Item.Color = color;
            return shape.Item;
        }

        private void Connect(InitArgs args)
        {
            _platformer.StartGame(args.LocalPort, args.IsLocal, args.Ip, args.RemotePort);
            _connected = true;
        }
        
        private InputMap HandleInput()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            
            InputMap input = new InputMap
            {
                LeftPressed = horizontal < 0,
                RightPressed = horizontal > 0,
                UpPressed = vertical > 0,
                DownPressed = vertical < 0,
                LeftMouseClicked = Input.GetAxis("Fire1") > 0,
            };
            return input;
        }
    }
}