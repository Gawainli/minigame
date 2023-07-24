using MiniGame.Asset;
using MiniGame.UI;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class TestAssetMem : MonoBehaviour
{
    public GameObject img;
    public Button btnLoad;
    public Button btnRemove;
    public Button btnUnload;
    public Button btnUnloadUnused;
    
    private void Awake()
    {
        btnLoad.onClick.AddListener(OnClickLoad);
        btnRemove.onClick.AddListener(OnClickRemove);
        btnUnload.onClick.AddListener(OnClickUnload);
        btnUnloadUnused.onClick.AddListener(OnClickUnloadUnused);
    }
    
    private async void OnClickLoad()
    {
        // img  = AssetModule.LoadGameObjectSync("Assets/_GameMain/Prefab/TestUIWindow.prefab", transform);
        await UIModule.OpenWindowAsync<TestUIDialog>("Assets/_GameMain/Prefab/TestUIWindow.prefab");
    }
    
    private void OnClickRemove()
    {
        // Destroy(img);
        // img = null;
        UIModule.CloseWindow<TestUIDialog>();
    }
    
    private void OnClickUnload()
    {
        var package = YooAssets.GetPackage("DefaultPackage");
        package.UnloadUnusedAssets();
    }
    
    private void OnClickUnloadUnused()
    {
        var package = YooAssets.GetPackage("DefaultPackage");
        package.ForceUnloadAllAssets();
    }
    
    
}