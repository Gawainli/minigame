using System;
using Cysharp.Threading.Tasks;
using MiniGame.Event;
using UnityEngine;
using MiniGame.Logger;

public class TestEvent : IEventMessage
{
    public string message;
}

public class MiniGameTest : MonoBehaviour
{
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
