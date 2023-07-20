using System;
using Cysharp.Threading.Tasks;
using MiniGame.Asset;
using MiniGame.Event;
using UnityEngine;
using MiniGame.Logger;
using MiniGame.Module;
using MiniGame.Pool;
using MiniGame.Scene;
using MiniGame.StateMachine;
using MiniGame.UI;
using UnityEngine.UI;
using YooAsset;

public class TestEvent : IEventMessage
{
    public string message;
}

public class MiniGameTest : MonoBehaviour
{
    private void Awake()
    {

    }

    // Start is called before the first frame update
    async void Start()
    {
        LogModule.Info("Test Info");
        LogModule.Warning("Test Warning");
        EventModule.AddListener<TestEvent>(OnEventTest);

        await UniTask.Delay(TimeSpan.FromSeconds(1));
        EventModule.SendEvent(new TestEvent { message = "Test Event" });
        
        // await UniTask.Delay(TimeSpan.FromSeconds(1));
        await PoolModule.CreateGameObjectPoolAsync("Assets/_GameMain/Prefab/Cube.prefab");
        PoolModule.Spawn("Assets/_GameMain/Prefab/Cube.prefab");

        // await UIModule.OpenWindowAsync<SampleTestMainWindow>("Assets/_GameMain/Prefab/SampleTestMainUI.prefab");

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
