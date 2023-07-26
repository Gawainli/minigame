using MiniGame.StateMachine;

namespace MiniGame.Runtime
{
    public class StatePatchDone : State
    {
        public override void Init()
        {
        }

        public override void Enter()
        {
            ChangeState<StateLoadAssembly>();
        }

        public override void Exit()
        {
        }
    }
}