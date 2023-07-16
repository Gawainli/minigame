using System;
using Cysharp.Threading.Tasks;
using MiniGame.Event;
using UnityEngine;
using MiniGame.Logger;
using MiniGame.Module;
using MiniGame.Resource;
using MiniGame.StateMachine;
using YooAsset;

public class TestEvent : IEventMessage
{
    public string message;
}

public class MiniGameTest : MonoBehaviour
{
    private void Awake()
    {
        ModuleCore.CreateModule<LogModule>();
        ModuleCore.CreateModule<StateMachineModule>();
        ModuleCore.CreateModule<EventModule>();

        var resCfg = new ResModuleCfg
        {
            packageName = "DefaultPackage",
            ePlayMode = EPlayMode.EditorSimulateMode,
            hostServerIP = "http://localhost:8080",
            appVersion = "v1.0"
        };

        ModuleCore.CreateModule<ResourceModule>(0, resCfg);
    }

    // Start is called before the first frame update
    async void Start()
    {
        LogModule.Info("Test Info");
        LogModule.Warning("Test Warning");
        EventModule.AddListener<TestEvent>(OnEventTest);

        await UniTask.Delay(TimeSpan.FromSeconds(1));
        EventModule.SendEvent(new TestEvent { message = "Test Event" });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEventTest(IEventMessage msg)
    {
        var testMsg = msg as TestEvent;
        LogModule.Info(testMsg.message);
    }
    
    private void OnDestroy()
    {
        EventModule.RemoveListener<TestEvent>(OnEventTest);
    }
}
