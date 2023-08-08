    using MiniGame.Asset;
    using MiniGame.Scene;
    using MiniGame.UI;
    using UnityEngine;
    using YooAsset;

    public class SampleTestMainWindow : UIWindow
    {
        private GameObject testSprite;
        public override void OnCreate()
        {
            var btn = Transform.Find("BtnOpenWindow").GetComponent<UnityEngine.UI.Button>();
            btn.onClick.AddListener(OnClickOpenTestUI);
            
            btn = Transform.Find("BtnCloseWindow").GetComponent<UnityEngine.UI.Button>();
            btn.onClick.AddListener(OnCloseTestUI);
            
            btn = Transform.Find("BtnUnloadUnused").GetComponent<UnityEngine.UI.Button>();
            btn.onClick.AddListener(OnUnload);
            
            btn = Transform.Find("BtnLoadEmptyScene").GetComponent<UnityEngine.UI.Button>();
            btn.onClick.AddListener(OnClickLoadEmptyScene);
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

        private async void OnClickOpenTestUI()
        {
            await UIModule.OpenWindowAsync<TestUIDialog>("Assets/_GameMain/Prefab/TestUIWindow.prefab");
        }

        private void OnCloseTestUI()
        {
            UIModule.CloseWindow<TestUIDialog>();
        }

        private void OnUnload()
        {
            AssetModule.UnloadUnusedAssets();
        }
        
        private async void OnClickLoadEmptyScene()
        {
            await SceneModule.ChangeSceneAsync("Assets/_GameMain/_Scenes/S_Main.unity");
        }
        
    }