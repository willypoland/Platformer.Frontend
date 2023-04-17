namespace Game.Code.Infrastructure.StateMachine
{
    public interface IParameterizedState<TParam> : IExitableState
    {
        void Enter(TParam param);
    }
}