using MiniGame.Asset;
using MiniGame.StateMachine;
using YooAsset;

namespace MiniGame.Base
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
                if (SettingUtils.playMode == EPlayMode.EditorSimulateMode || SettingUtils.playMode == EPlayMode.OfflinePlayMode)
                {
                    ChangeState<StateStartGame>();
                }
                else
                {
                    ChangeState<StateUpdateVersion>();
                }
            }
        }

        public override void Exit()
        {
        }
    }
}