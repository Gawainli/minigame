using MiniGame.StateMachine;

namespace MiniGame.Runtime
{
    public class StatePatchPrepare : State
    {
        public override void Init()
        {
            
        }

        public override void Enter()
        {
            ChangeState<StateInitPackage>();
        }

        public override void Exit()
        {
        }
    }
}