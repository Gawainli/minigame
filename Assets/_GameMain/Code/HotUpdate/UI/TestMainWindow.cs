using MiniGame.Scene;
using MiniGame.UI;
using UnityEngine.UI;

public class TestMainWindow : UIWindow
{
    public override void OnCreate()
    {
        var btnMemTest = this.Q<Button>("Panel/BtnMemTest");
        btnMemTest.onClick.AddListener(OnClickMemTest);
        var btnLubanTest = this.Q<Button>("Panel/BtnLubanTest");
        btnLubanTest.onClick.AddListener(OnClickLubanTest);
        var btnTestProto = this.Q<Button>("Panel/BtnProtoTest");
        btnTestProto.onClick.AddListener(OnClickTestProto);
    }

    public override void OnRefresh()
    {
    }

    public override void OnUpdate(float deltaTime)
    {
    }

    public override void OnDestroy()
    {
    }
    
    private async void OnClickMemTest()
    {
        UIModule.CloseWindow<TestMainWindow>();
        await SceneModule.ChangeSceneAsync("Assets/_GameMain/_Scenes/S_TestMemory.unity");
    }
    
    private async void OnClickLubanTest()
    {
        UIModule.CloseWindow<TestMainWindow>();
        await SceneModule.ChangeSceneAsync("Assets/_GameMain/_Scenes/S_TestLuban.unity");
    }
    
    private async void OnClickTestProto()
    {
        UIModule.CloseWindow<TestMainWindow>();
        await SceneModule.ChangeSceneAsync("Assets/_GameMain/_Scenes/S_TestProto.unity");
    }
}