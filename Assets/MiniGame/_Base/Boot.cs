using System;
using MiniGame.Asset;
using MiniGame.Event;
using MiniGame.Logger;
using MiniGame.Module;
using MiniGame.Pool;
using MiniGame.Scene;
using MiniGame.StateMachine;
using MiniGame.UI;
using UnityEngine;
using YooAsset;

namespace MiniGame.Base
{
    public class Boot : MonoBehaviour, IModule
    {
        public EPlayMode resPlayMode = EPlayMode.EditorSimulateMode;
        public string packageName = "DefaultPackage";
        public string hostServerIP = "http://localhost:8080";
        public string appVersion = "v1.0";

        public GameObject uiRoot;

        private StateMachine.StateMachine _stateMachine;

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
            ModuleCore.CreateModule<AssetModule>(resCfg);
            ModuleCore.CreateModule<PoolModule>(new PoolModuleCfg()
            {
                pkg = AssetModule.pkg,
                poolingRoot = gameObject
            });
            ModuleCore.CreateModule<UIModule>(uiRoot);

            _stateMachine = StateMachineModule.Create<Boot>(this);
            _stateMachine.AddState<StatePatchPrepare>();
            _stateMachine.AddState<StateInitPackage>();
            _stateMachine.AddState<StateUpdateVersion>();
            _stateMachine.AddState<StateUpdateManifest>();
            _stateMachine.AddState<StateCreateDownloader>();
            _stateMachine.AddState<StateDownloadFile>();
            _stateMachine.AddState<StateClearCache>();
            _stateMachine.AddState<StatePatchDone>();
            _stateMachine.AddState<StateStartGame>();
            _stateMachine.AddState<StateLoadAssembly>();
            
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

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            _stateMachine.Start<StatePatchPrepare>();
        }
        
        private void Update()
        {
            Tick(Time.deltaTime, Time.unscaledDeltaTime);
        }

        #endregion
    }
}