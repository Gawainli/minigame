using System.Reflection;
using Cysharp.Threading.Tasks;
using MiniGame.Asset;
using MiniGame.Logger;
using MiniGame.StateMachine;

namespace MiniGame.Base
{
    public class StateLoadAssembly : State
    {
        public override void Init()
        {
        }

        public override async void Enter()
        {
            LogModule.Info("StateLoadAssembly");
            var data = await AssetModule.LoadRawFileAsync("Assets/HotUpdateDll/ADF_Base.dll.bytes");
            var hotUpdate = Assembly.Load(data);
            var type = hotUpdate.GetType("HotUpdateHelper");
            var result = await (UniTask<bool>)type.GetMethod("LoadAllHotUpdate")?.Invoke(null, null);
            if (result)
            {
                ChangeState<StateStartGame>();
            }
        }

        public override void Exit()
        {
        }
    }
}