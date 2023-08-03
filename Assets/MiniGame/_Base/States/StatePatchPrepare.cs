using MiniGame.StateMachine;

namespace MiniGame.Base
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