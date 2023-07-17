using System;
using Cysharp.Threading.Tasks;
using MiniGame.Event;
using UnityEngine;
using MiniGame.Logger;
using MiniGame.Module;
using MiniGame.Resource;
using MiniGame.StateMachine;
using UnityEngine.UI;
using YooAsset;

public class TestEvent : IEventMessage
{
    public string message;
}

public class MiniGameTest : MonoBehaviour
{
    public Image img;
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
        var sp = ResourceModule.LoadAssetSync<Sprite>("Assets/_GameMain/Prefab/TestSprite.png");
        img.sprite = sp;
        img.SetNativeSize();

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
