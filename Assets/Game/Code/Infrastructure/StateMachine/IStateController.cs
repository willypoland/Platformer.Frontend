namespace Game.Code.Infrastructure.StateMachine
{
    public interface IStateController
    {
        void Add<TState>(TState state) where TState : class, IExitableState;
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TParam>(TParam param) where TState : class, IParameterizedState<TParam>;
    }
}