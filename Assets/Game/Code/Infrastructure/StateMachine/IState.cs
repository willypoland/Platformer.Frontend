namespace Game.Code.Infrastructure.StateMachine
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}