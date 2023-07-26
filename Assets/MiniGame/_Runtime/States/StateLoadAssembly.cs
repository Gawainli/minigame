using MiniGame.Logger;
using MiniGame.StateMachine;

namespace MiniGame.Runtime
{
    public class StateLoadAssembly : State
    {
        public override void Init()
        {
        }

        public override void Enter()
        {
            LogModule.Info("StateLoadAssembly");
            ChangeState<StateStartGame>();
        }

        public override void Exit()
        {
        }
    }
}