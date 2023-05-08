using System;
using System.Linq;
using Api;
using Game.Scripts.Common;
using Game.Scripts.Data;
using Game.Scripts.Infrastructure;
using Game.Scripts.Plot;
using UnityEngine;
using UnityEngine.UI;


namespace Game.Scripts.Logic
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private Text _tickFps;
        [SerializeField] private Text _drawFps;
        [SerializeField] private PlotView _plotDx;
        
        [SerializeField] private Transform _platformsRoot;
        [SerializeField] private Transform _playersRoot;
        [SerializeField, Range(0, 1)] private int _activePlayer;

        private PlayerView[] _playerViews;
        private PlatformView[] _platforms;
        private GameConfig _config;

        private readonly byte[] _buffer = new byte[2048];
        private SceneConverter _converter;
        private IApi _api;
        private IGameState _gs;

        private float _prevTickTime;
        private float _lastTickTime;

        private float _dx2;
        private float _dx1;
        private float _dx0;
        private float _dx;

        private int _prevFrame;

        private void Start()
        {
            _plotDx.SetMinMax(0f, 2f);
            
            _playerViews = _playersRoot.GetComponentsInChildren<PlayerView>();
            _platforms = _platformsRoot.GetComponentsInChildren<PlatformView>();
            
            _config = Resources.Load<GameConfig>(AssetPath.GameConfig);
            _converter = new SceneConverter(_config);

            Location location = new()
            {
                IsFirstPlayer = _activePlayer == 0,
                PositionFirstPlayer = _converter.ToCoreRect(_playerViews[0].GameObjectView.ToViewRect()).position,
                PositionSecondPlayer = _converter.ToCoreRect(_playerViews[1].GameObjectView.ToViewRect()).position,
                Platfroms = _platforms.Select((x, i) => _converter.ToCorePlatform(i, x.Type, x.Rect)).ToArray(),
            };

            // _api = ApiFactory.CreateApiAsync();
            _api = ApiFactory.CreateApiSync();
            _gs = ApiFactory.CreateGameState();
            _api.Init(location);
            _api.StartGame();
        }

        private void Update()
        {
            if (_api.GetStatus() != GameStatus.RUN)
                return;

            var input = GatherInput();
            _api.Update(input);

            _api.GetState(_buffer, out int len, out float dx);
            _gs.Update(_buffer, len);

            if (_gs.Frame != _prevFrame)
            {
                _dx2 = _dx1;
                _dx1 = _dx0;
                _dx0 = dx;
                _dx = CalcDx();
                
                _prevTickTime = _lastTickTime;
                _lastTickTime = Time.realtimeSinceStartup;
                UpdatePlayersView(_gs.Players, _dx);


                _prevFrame = _gs.Frame;
            }

            UpdateGUI();
            UpdateInputHelper();
        }

        private void OnDisable()
        {
            _api.StopGame();
        }

        private void UpdateInputHelper()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                QualitySettings.vSyncCount = (QualitySettings.vSyncCount + 1) % 3;
            }
        }

        private void UpdateGUI()
        {
            _plotDx.Push(_dx);
            _tickFps.text = _dx.ToString("F2");
            _drawFps.text = (1f / Time.deltaTime).ToString("F2");
        }

        private void UpdatePlayersView(ReadOnlySpan<IPlayer> players, float dx)
        {
            int count = Mathf.Min(players.Length, _playerViews.Length);

            for (int i = 0; i < count; i++)
            {
                var player = players[i];
                var view = _playerViews[i];
                
                var obj = player.Object;
                var rect = new RectInt(obj.Position.RoundToInt(), obj.Size.RoundToInt());
                var viewRect = _converter.ToViewRect(rect);
                view.InterpolatedGameObjectView.SetPosition(viewRect.position, _lastTickTime, dx);
                view.GameObjectView.SetViewRect(viewRect);
                view.Animator.UpdateState(player);

                if (_activePlayer == i)
                {
                    Debug.Log($"{player.State} {player.StateFrame}");
                }
            }
        }

        private static InputMap GatherInput()
        {
            InputMap map = new()
            {
                LeftPressed = Input.GetKey(KeyCode.A),
                RightPressed = Input.GetKey(KeyCode.D),
                UpPressed = Input.GetKey(KeyCode.W),
                DownPressed = Input.GetKey(KeyCode.S),
                LeftMouseClicked = Input.GetKey(KeyCode.Mouse0),
                RightMouseClicked = Input.GetKey(KeyCode.Mouse1),
            };

            return map;
        }

        private float CalcDx()
        {
            // float d = 0.5f;
            // return Mathf.Lerp(Mathf.Lerp(_dx2, _dx1, d), Mathf.Lerp(_dx1, _dx0, d), d);
            // _currentDx = Mathf.Lerp(_currentDx, _dx0, Time.deltaTime * 2f);
            return _dx0;
        }
    }


}