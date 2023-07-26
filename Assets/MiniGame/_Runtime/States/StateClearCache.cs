using Cysharp.Threading.Tasks;
using MiniGame.Asset;
using MiniGame.StateMachine;

namespace MiniGame.Runtime
{
    public class StateClearCache : State
    {
        public override void Init()
        {
        }

        public override async void Enter()
        {
            await AssetModule.pkg.ClearAllCacheFilesAsync().ToUniTask();
            ChangeState<StatePatchDone>();
        }

        public override void Exit()
        {
        }
    }
}