using MiniGame.Scene;
using MiniGame.UI;

public class TestMainWindow : UIWindow
{
    public override void OnCreate()
    {
        var btnMemTest = Transform.Find("Panel/BtnMemTest").GetComponent<UnityEngine.UI.Button>();
        btnMemTest.onClick.AddListener(OnClickMemTest);
        var btnLubanTest = Transform.Find("Panel/BtnLubanTest").GetComponent<UnityEngine.UI.Button>();
        btnLubanTest.onClick.AddListener(OnClickLubanTest);
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
        await SceneModule.ChangeSceneAsync("Assets/_GameMain/_Scenes/S_LubanTest.unity");
    }
}