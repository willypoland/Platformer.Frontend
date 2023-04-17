using System;
using Api;
using Game.Code.Data;
using Game.Code.Data.Settings;
using Game.Code.Infrastructure.Extensions;
using Game.Code.Infrastructure.StateMachine;
using Game.Code.Services;
using Game.Code.UI;
using Game.Code.UI.Interfaces;


namespace Game.Code.Logic
{
    public sealed class ConnectionState : IState
    {
        private readonly GameSettingsService _settingsService;
        private readonly IStateController _stateController;
        private readonly IConnectionSetupView _view;
        private readonly ConnectionArguments _arguments;

        public ConnectionState(GameSettingsService settingsService,
                               IStateController stateController,
                               IConnectionSetupView view,
                               ConnectionArguments arguments)
        {
            _settingsService = settingsService;
            _stateController = stateController;
            _view = view;
            _arguments = arguments;
        }

        public void Enter()
        {
            _view.InputChanged += ViewOnInputChanged;
            _view.ClickConnection += ViewOnClickConnection;
            _arguments.AnyChanged += ArgumentsOnAnyChanged;
            LoadConnectionSettnigs();
        }

        private void LoadConnectionSettnigs()
        {
            var args = Environment.GetCommandLineArgs();

            if (ConnectionArguments.ValidateComandLineArgument(args, out bool isMaster, out var localPort,
                out var remoteIp, out var remotePort))
            {
                _arguments.IsMaster = isMaster;
                _arguments.LocalPort = localPort;
                _arguments.RemoteIp = remoteIp;
                _arguments.RemotePort = remotePort;
                return;
            }

            var init = _settingsService.Read().InitialConnection;
            _arguments.IsMaster = init.IsMaster;
            _arguments.LocalPort = init.LocalPort;
            _arguments.RemoteIp = init.RemoteIp;
            _arguments.RemotePort = init.RemotePort;
            ArgumentsOnAnyChanged();
        }

        private void ArgumentsOnAnyChanged()
        {
            var input = new InputConnectionArguments();
            input.IsMaster = _arguments.IsMaster;
            input.LocalPort.Field = _arguments.LocalPort.ToString();
            input.RemoteAddress.Field = _arguments.RemoteIp + ":" + _arguments.RemotePort;
            _view.SetArguments(input);
        }

        public void Exit()
        {
            _view.InputChanged -= ViewOnInputChanged;
            _view.ClickConnection -= ViewOnClickConnection;
            _arguments.AnyChanged -= ArgumentsOnAnyChanged;
        }

        private InputConnectionArguments ViewOnInputChanged(InputConnectionArguments arg)
        {
            arg.LocalPort.IsValid = true;
            arg.RemoteAddress.IsValid = true;
            
            if (!ConnectionArguments.ValidatePort(arg.LocalPort.Field, out var localPort))
            {
                arg.LocalPort.IsValid = false;
                return arg;
            }
            
            if (!ConnectionArguments.ValidateAddress(arg.RemoteAddress.Field, out var remoteIp, out var remotePort))
            {
                arg.RemoteAddress.IsValid = false;
                return arg;
            }

            _arguments.IsMaster = arg.IsMaster;
            _arguments.LocalPort = localPort;
            _arguments.RemoteIp = remoteIp;
            _arguments.RemotePort = remotePort;

            return arg;
        }

        private void ViewOnClickConnection()
        {
#if !UNITY_EDITOR
            var args = Environment.GetCommandLineArgs();
            BackendType type;
            if (args[1].Contains("sync"))
            {
                type = BackendType.Sync;
            }
            else if (args[1].Contains("async"))
            {
                type = BackendType.Async;
            }
            else if (args[1].Contains("ggpo"))
            {
                type = BackendType.GGPO;
            }
            else
            {
                type = _settingsService.Read().Other.BackendType;
            }
#else
            BackendType type = _settingsService.Read().Other.BackendType;
#endif

            IApi api = type switch
            {
                BackendType.Sync => ApiFactory.CreateApiSync(),
                BackendType.Async => ApiFactory.CreateApiAsync(),
                BackendType.GGPO => ApiFactory.CreateApiGGPO(),
                _ => throw new ArgumentOutOfRangeException()
            };

            IGameState gs = ApiFactory.CreateGameState();
            
            api.RegisterPeer(_arguments.LocalPort, _arguments.IsMaster, _arguments.RemoteIp, _arguments.RemotePort);

            var result = _stateController.EnterInNextFrame<GameplayState, (IApi, IGameState)>((api, gs));
        }
    }
}