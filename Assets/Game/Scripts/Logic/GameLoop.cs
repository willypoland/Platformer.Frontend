using System;
using Api;
using Game.Scripts.Common;
using Game.Scripts.Data;
using Game.Scripts.Infrastructure;
using UnityEngine;


namespace Game.Scripts.Logic
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private TransformMarker _player1Pos;
        [SerializeField] private TransformMarker _player2Pos;
        [SerializeField] private Transform _platformsRoot;

        private readonly byte[] _buffer = new byte[512];
        private SceneConverter _converter;
        private IApi _api;
        private IGameState _gs;

        private void Start()
        {
            var config = Resources.Load<GameConfig>(AssetPath.GameConfig);
            _converter = new SceneConverter(config);

            // SetupTestScene(_player1Pos.transform.parent, _converter,
            //     new RectInt(192, 702, 0, 0),
            //     new RectInt(96, 704, 0, 0));
            //
            // SetupTestScene(_platformsRoot, _converter,
            //     new RectInt(0, 864, 864, 32),
            //     new RectInt(256, 608, 192, 32),
            //     new RectInt(672, 736, 224, 32),
            //     new RectInt(0, 640, 32, 256),
            //     new RectInt(864, 640, 32, 256));

            Location location = new()
            {
                IsFirstPlayer = true,
                PositionFirstPlayer = _converter.ToCoreRect(_player1Pos.ToViewRect()).position,
                PositionSecondPlayer = _converter.ToCoreRect(_player2Pos.ToViewRect()).position,
                Platfroms = GatherPlatforms(_platformsRoot, _converter),
            };

            _api = ApiFactory.CreateApiSync();
            _gs = ApiFactory.CreateGameState();
            _api.Init(location);
            _api.StartGame();
        }

        private void Update()
        {
            var status = _api.GetStatus();

            if (ShouldSkipFrame(status))
                return;

            var input = GatherInput();
            _api.Update(input);

            int len = _api.GetState(_buffer);
            _gs.Update(_buffer, len);

            UpdatePlayersView(_gs.Players);
        }

        private void UpdatePlayersView(ReadOnlySpan<IPlayer> players)
        {
            var rect1 = new RectInt(players[0].Object.Position.RoundToInt(), players[0].Object.Size.RoundToInt());
            var rect2 = new RectInt(players[1].Object.Position.RoundToInt(), players[1].Object.Size.RoundToInt());
            _player1Pos.SetViewRect(_converter.ToViewRect(rect1));
            _player2Pos.SetViewRect(_converter.ToViewRect(rect2));
        }

        private void OnDisable()
        {
            _api.StopGame();
        }

        private static bool ShouldSkipFrame(GameStatus status)
        {
            return status switch
            {
                GameStatus.INVALID => true,
                GameStatus.RUN => false,
                GameStatus.SYNC => true,
                GameStatus.STOPED => true,
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
        }

        private static Platform[] GatherPlatforms(Transform root, SceneConverter converter)
        {
            var markers = root.GetComponentsInChildren<PlatformMarker>();
            int count = markers.Length;
            var platforms = new Platform[count];

            for (int i = 0; i < count; i++)
                platforms[i] = converter.ToCorePlatform(i, markers[i].Type, markers[i].Rect);

            return platforms;
        }

        public static void SetupTestScene(Transform platforms, SceneConverter converter, params RectInt[] coreRect)
        {
            var markers = platforms.GetComponentsInChildren<TransformMarker>();
            int len = Math.Min(coreRect.Length, markers.Length);

            for (int i = 0; i < len; i++)
                markers[i].SetViewRect(converter.ToViewRect(coreRect[i]));
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