using Game.Code.Infrastructure.StateMachine;
using UnityEngine;


namespace Game.Code.Infrastructure
{
    public sealed class ParseCommandLineArgumentState : IState
    {

        public void Enter()
        {
            Debug.Log("Enter to Parse command line argument state");
        }

        public void Exit()
        {
        }
    }
}