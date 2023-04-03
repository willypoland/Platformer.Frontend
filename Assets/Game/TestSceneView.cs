using System;
using System.Collections.Generic;
using Api;
using Roz.Common.Pool;
using Ser;
using TMPro;
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
        [SerializeField] private PlayerView[] _playerViews;

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
            _shapes.ReleaseAll();

            for (int i = 0; i < gs.Players.Count; i++)
            {
                var playerData = gs.Players[i];
                var shape = GetShape(_playerColor);
                var obj = playerData.Obj;
                shape.Set(obj.Position.X, obj.Position.Y, obj.Width, obj.Height);
                _playerViews[i].Set(playerData, shape);
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
            if (_connected)
                _platformer.StopGame();
            _platformer.StartGame(args.LocalPort, args.IsLocal, args.Ip, args.RemotePort);
            _connected = true;
        }
        
        private InputMap HandleInput()
        {
            InputMap input = new InputMap
            {
                LeftPressed = Input.GetKey(KeyCode.A),
                RightPressed = Input.GetKey(KeyCode.D),
                UpPressed = Input.GetKey(KeyCode.W),
                DownPressed = Input.GetKey(KeyCode.S),
                LeftMouseClicked = Input.GetMouseButton(0),
            };
            return input;
        }
    }
}