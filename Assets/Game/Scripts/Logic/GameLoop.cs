using System;
using System.Linq;
using Api;
using Game.Scripts.Common;
using Game.Scripts.Data;
using Game.Scripts.Infrastructure;
using UnityEngine;
using UnityEngine.UI;


namespace Game.Scripts.Logic
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private Text _tickFps;
        [SerializeField] private Text _drawFps;
        
        [SerializeField] private Transform _platformsRoot;
        [SerializeField] private Transform _playersRoot;
        [SerializeField, Range(0, 1)] private int _activePlayer = 0;

        private InterpolatedGameObjectView[] _players;
        private PlatformView[] _platforms;
        private CameraPursuer _cameraPursuer;
        private GameConfig _config;

        private readonly byte[] _buffer = new byte[2048];
        private SceneConverter _converter;
        private IApi _api;
        private IGameState _gs;

        private float _prevTickTime;
        private float _lastTickTime;

        private int _prevFrame;

        private void Start()
        {
            _players = _playersRoot.GetComponentsInChildren<InterpolatedGameObjectView>();
            _platforms = _platformsRoot.GetComponentsInChildren<PlatformView>();
            _cameraPursuer = FindObjectOfType<CameraPursuer>();
            
            _config = Resources.Load<GameConfig>(AssetPath.GameConfig);
            _converter = new SceneConverter(_config);

            Location location = new()
            {
                IsFirstPlayer = _activePlayer == 0,
                PositionFirstPlayer = _converter.ToCoreRect(_players[0].GameObjectView.ToViewRect()).position,
                PositionSecondPlayer = _converter.ToCoreRect(_players[1].GameObjectView.ToViewRect()).position,
                Platfroms = _platforms.Select((x, i) => _converter.ToCorePlatform(i, x.Type, x.Rect)).ToArray(),
            };

            _api = ApiFactory.CreateApiAsync();
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

            int len = _api.GetState(_buffer);
            _gs.Update(_buffer, len);

            if (_gs.Frame != _prevFrame)
            {
                _prevTickTime = _lastTickTime;
                _lastTickTime = Time.realtimeSinceStartup;
                UpdatePlayersView(_gs.Players);
                _prevFrame = _gs.Frame;
            }

            UpdateCamera();

            UpdateGUI();
            UpdateInputHelper();
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
            _tickFps.text = (1f / (_lastTickTime - _prevTickTime)).ToString("F2");
            _drawFps.text = (1f / Time.deltaTime).ToString("F2");
        }

        private void OnDisable()
        {
            _api.StopGame();
        }

        private void UpdatePlayersView(ReadOnlySpan<IPlayer> players)
        {
            int count = Mathf.Min(players.Length, _players.Length);

            for (int i = 0; i < count; i++)
            {
                var obj = players[i].Object;
                var rect = new RectInt(obj.Position.RoundToInt(), obj.Size.RoundToInt());
                var viewRect = _converter.ToViewRect(rect);
                _players[i].SetPosition(viewRect.position, _lastTickTime, _lastTickTime - _prevTickTime);
            }
        }

        private void UpdateCamera()
        {
            Vector2 center = Vector2.zero;

            foreach (var player in _players)
                center += (Vector2) player.transform.position;

            //center /= _players.Length;

            _cameraPursuer.Target = _players[0].transform.position;
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
    }


}