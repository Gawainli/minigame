using System;
using MiniGame.Asset;
using MiniGame.Event;
using MiniGame.Logger;
using MiniGame.Module;
using MiniGame.Scene;
using MiniGame.StateMachine;
using UnityEngine;
using YooAsset;

namespace MiniGame.Base
{
    public class GameEntry : MonoBehaviour, IModule
    {
        public EPlayMode resPlayMode = EPlayMode.EditorSimulateMode;
        public string packageName = "DefaultPackage";
        public string hostServerIP = "http://localhost:8080";
        public string appVersion = "v1.0";

        public void Initialize(object userData = null)
        {
            ModuleCore.CreateModule<LogModule>();
            ModuleCore.CreateModule<StateMachineModule>();
            ModuleCore.CreateModule<EventModule>();

            var resCfg = new AssetModuleCfg
            {
                packageName = packageName,
                ePlayMode = resPlayMode,
                hostServerIP = hostServerIP,
                appVersion = appVersion
            };
            ModuleCore.CreateModule<AssetModule>(0, resCfg);
            
            Initialized = true;
        }

        public void Tick(float deltaTime, float unscaledDeltaTime)
        {
            ModuleCore.TickAllModules(deltaTime, unscaledDeltaTime);
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public int Priority { get; set; } = 9999;
        public bool Initialized { get; set; }

        #region Mono
        private async void Awake()
        {
            Initialize();
            await AssetModule.Instance.InitPkgAsync(); 
            await SceneModule.ChangeSceneAsync("Assets/_GameMain/_Scenes/S_Splash.unity");
        }

        private async void Start()
        {
        }


        private void Update()
        {
            Tick(Time.deltaTime, Time.unscaledDeltaTime);
        } 

        #endregion

    }
}