using System;
using MiniGame.Base;
using MiniGame.Event;
using MiniGame.Logger;
using MiniGame.Resource;
using MiniGame.StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace MiniGame.Module
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

            var resCfg = new ResModuleCfg
            {
                packageName = packageName,
                ePlayMode = resPlayMode,
                hostServerIP = hostServerIP,
                appVersion = appVersion
            };
            ModuleCore.CreateModule<ResourceModule>(0, resCfg);
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

        #region Mono
        private void Awake()
        {
            Initialize();
        }
        private void Update()
        {
            Tick(Time.deltaTime, Time.unscaledDeltaTime);
        } 

        #endregion

    }
}