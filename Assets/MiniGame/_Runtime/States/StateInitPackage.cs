using MiniGame.Asset;
using MiniGame.StateMachine;

namespace MiniGame.Runtime
{
    public class StateInitPackage : State
    {
        public override void Init()
        {
        }

        public override async void Enter()
        {
            var succeed = await AssetModule.InitPkgAsync();
            if (succeed)
            {
                ChangeState<StateUpdateVersion>();
            }
        }

        public override void Exit()
        {
        }
    }
}