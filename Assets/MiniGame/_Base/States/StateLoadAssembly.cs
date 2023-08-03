using System.Reflection;
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
            var data = await AssetModule.LoadRawFileAsync("Assets/HotUpdateDll/Hotfix.dll.bytes");
            var hotUpdate = Assembly.Load(data);
            var type = hotUpdate.GetType("MiniGame.Sample.HotUpdateHelper");
            var result = type.GetMethod("LoadAllHotUpdate")?.Invoke(null, null);
            LogModule.Info(result?.ToString());
        }

        public override void Exit()
        {
        }
    }
}