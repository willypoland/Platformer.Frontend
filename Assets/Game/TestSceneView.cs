using System;
using System.Collections.Generic;
using Api;
using CsUtility.Pool;
using Ser;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Game
{

    public class TestSceneView : MonoBehaviour
    {
        [SerializeField] private Color _playerColor = Color.white;
        [SerializeField] private Color _attackColor = Color.red;
        [SerializeField] private Color _platformColor = Color.yellow;
        [SerializeField] private ShapeMock _shapePrefab;
        [SerializeField] private Transform _sceneRoot;
        [Header("Views")]
        [SerializeField] private InitializeFieldView _initializeFieldView;
        [SerializeField] private InputMapView _inputMapView;
        [SerializeField] private StatusView _statusView;
        [SerializeField] private PlayerView[] _playerViews;
        [SerializeField] private Text _frameText;

        private PlatformerCore _platformer;
        private bool _connected = false;

        private ShapeMock.Factory _shapeFactroy;
        private ObjectPool<ShapeMock> _shapes;

        private int _prevFrame;

        private void Start()
        {
            _shapeFactroy = new ShapeMock.Factory(_sceneRoot, _shapePrefab);
            _shapes = new ObjectPool<ShapeMock>(_shapeFactroy);
            _shapes.PrepareInstances(10);

#if !UNITY_EDITOR
            if (Environment.GetCommandLineArgs().Length > 1)
                _platformer = PlatformerCore.CreateGGPO();
            else
                _platformer = PlatformerCore.CreateAsync();
#else
            _platformer = PlatformerCore.CreateAsync();
#endif
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

                var prevFrame = _platformer.GameState?.Frame ?? 0;
                var status = _platformer.UpdateTick(input);
                var currFrame = _platformer.GameState?.Frame ?? 0;
                var sameFrame = currFrame == prevFrame;
                
                _frameText.text = currFrame.ToString();

                _statusView.SetStatus(Map(status));

                if (_platformer.GameState != null && !sameFrame)
                    UpdateScene(_platformer.GameState);
            }
        }

        private StatusView.Status Map(GameStatus status)
        {
            return status switch
            {
                GameStatus.RUN => StatusView.Status.Run,
                GameStatus.SYNC => StatusView.Status.Sync,
                GameStatus.STOPED => StatusView.Status.Stopped,
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null),
            };
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