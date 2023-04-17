using System;
using Game.Code.Infrastructure.Extensions;
using Game.Code.Infrastructure.StateMachine;
using Game.Code.Services;
using UnityEngine;


namespace Game.Code.Infrastructure
{
    public sealed class GameInitializationState : IState
    {
        private readonly IStateController _stateController;
        private readonly GameSettingsService _gameSettingsService;

        public GameInitializationState(IStateController stateController, GameSettingsService gameSettingsService)
        {
            _stateController = stateController;
            _gameSettingsService = gameSettingsService;
        }

        public void Enter()
        {
            var args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
            {
            }

            var result = _stateController.EnterInNextFrame<ParseCommandLineArgumentState>();
        }

        public void Exit()
        {
        }
    }


}