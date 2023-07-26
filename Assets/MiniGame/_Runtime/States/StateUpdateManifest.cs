using MiniGame.Asset;
using MiniGame.StateMachine;

namespace MiniGame.Runtime
{
    public class StateUpdateManifest : State
    {
        public override void Init()
        {
        }

        public override async void Enter()
        {
            var succeed = await AssetModule.UpdateManifest();
            if (succeed)
            {
                ChangeState<StateCreateDownloader>();
            }
        }

        public override void Exit()
        {
        }
    }
}