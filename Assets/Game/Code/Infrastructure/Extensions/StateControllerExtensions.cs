using Cysharp.Threading.Tasks;
using Game.Code.Infrastructure.StateMachine;


namespace Game.Code.Infrastructure.Extensions
{
    public static class StateControllerExtensions
    {
        public static async UniTaskVoid EnterInNextFrame<TState>(this IStateController self)
            where TState : class, IState
        {
            await UniTask.NextFrame();
            self.Enter<TState>();
        }

        public static async UniTaskVoid EnterInNextFrame<TState, TParam>(this IStateController self, TParam param)
            where TState : class, IParameterizedState<TParam>
        {
            await UniTask.NextFrame();
            self.Enter<TState, TParam>(param);
        }
    }
}