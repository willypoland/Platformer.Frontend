using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using VContainer;


namespace Game.Code.Infrastructure.StateMachine
{
    public class GameStateController : IStateController
    {
        private readonly IObjectResolver _resolver;
        private Dictionary<Type, IExitableState> _states = new();
        private IExitableState _activeState;

        public GameStateController(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public bool IsActive<TState>() where TState : class, IExitableState
        {
            return _activeState is TState;
        }

        public void Add<TState>(TState state) where TState : class, IExitableState
        {
            _states.Add(typeof(TState), state);
        }

        public void Enter<TState>() where TState : class, IState
        {
            ChangeState<TState>().Enter();
        }

        public void Enter<TState, TParam>(TParam param) where TState : class, IParameterizedState<TParam>
        {
            ChangeState<TState>().Enter(param);
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            var type = typeof(TState);

            if (!_states.TryGetValue(type, out IExitableState state))
            {
                state = _resolver.Resolve(type) as IExitableState;
                _states.Add(type, state);
            }
            
            return state as TState;
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            var state = GetState<TState>();
            _activeState = state;
            return state;
        }
    }
}