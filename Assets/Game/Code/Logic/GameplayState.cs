using System.Collections;
using Api;
using Game.Code.Data;
using Game.Code.Data.Settings;
using Game.Code.Infrastructure.Extensions;
using Game.Code.Infrastructure.Services;
using Game.Code.Infrastructure.StateMachine;
using Game.Code.Services;
using Game.Code.UI.Interfaces;
using VContainer.Unity;


namespace Game.Code.Logic
{
    public class GameplayState : IParameterizedState<(IApi, IGameState)>
    {
        private const int RawBufferSize = 512;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IStateController _stateController;
        private readonly IStatusView _statusView;
        private readonly IViewObjectFactory _viewFactroy;
        private readonly IInputService _inputService;
        private readonly GameConfig _gameConfig;
        private readonly GameSettingsService _settingsService;
        private readonly IConnectionSetupView _connectionSetupView;
        private IGameState _gameState;
        private IApi _api;

        private readonly byte[] _buffer = new byte[RawBufferSize];
        private bool _showPlayerShape;

        public GameplayState(ICoroutineRunner coroutineRunner,
                             IStateController stateController, 
                             IStatusView statusView,
                             IViewObjectFactory viewObjectFactory,
                             IInputService inputService, 
                             GameConfig gameConfig,
                             GameSettingsService settingsService,
                             IConnectionSetupView connectionSetupView)
        {
            _coroutineRunner = coroutineRunner;
            _stateController = stateController;
            _statusView = statusView;
            _viewFactroy = viewObjectFactory;
            _inputService = inputService;
            _gameConfig = gameConfig;
            _settingsService = settingsService;
            _connectionSetupView = connectionSetupView;
        }

        private void SettingsChanged(GameSettings settings)
        {
            _showPlayerShape = settings.Other.ShowPlayerShape;
        }

        public void Enter((IApi, IGameState) param)
        {
            var (api, gs) = param;
            _api = api;
            _gameState = gs;
            
            _settingsService.Changed += SettingsChanged;
            _connectionSetupView.ClickConnection += ClickConnection;
            SettingsChanged(_settingsService.Read());

            _coroutineRunner.StartCoroutine(Tick());
        }

        public void Exit()
        {
            _api.StopGame();
            _coroutineRunner.StopAllCoroutines();
            _settingsService.Changed -= SettingsChanged;
            _connectionSetupView.ClickConnection -= ClickConnection;
        }

        private IEnumerator Tick()
        {
            _api.StartGame();
            while (true)
            {
                yield return null;
                
                if (UpdateStatus() == GameStatus.RUN)
                {
                    UpdateData();
                    UpdateScene();
                }
            }
        }

        private void UpdateData()
        {
            var input = _inputService.ReadInputMap();
            _api.Update(input);
            int count = _api.GetState(_buffer);
            _gameState.Update(_buffer, count);
        }

        private void UpdateScene()
        {
            _viewFactroy.Clear();

            foreach (var player in _gameState.Players)
            {
                if (_showPlayerShape)
                    _viewFactroy.ShowShape(player.Object, _gameConfig.PlayerShapeColor);
                _viewFactroy.ShowPlayer(player);
            }

            foreach (var platform in _gameState.Platforms)
            {
                _viewFactroy.ShowShape(platform, _gameConfig.PlatformShapeColor);
            }

            foreach (var attack in _gameState.MeleeAttacks)
            {
                _viewFactroy.ShowShape(attack, _gameConfig.AttackShapeColor);
            }
        }

        public void ClickConnection()
        {
            var result = _stateController.EnterInNextFrame<ConnectionState>();
        }

        private GameStatus UpdateStatus()
        {
            var status = _api.GetStatus();

            switch (status)
            {
            case GameStatus.RUN:
                _statusView.Ok();
                break;
            case GameStatus.SYNC:
                _statusView.Warning();
                break;
            case GameStatus.STOPED:
                _statusView.Inactive();
                break;
            default:
                _statusView.Error();
                break;
            }

            return status;
        }
    }


}