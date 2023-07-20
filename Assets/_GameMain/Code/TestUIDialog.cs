    using MiniGame.Logger;
    using MiniGame.UI;
    using UnityEngine;

    public class TestUIDialog : UIWindow
    {
        public override void OnCreate()
        {
            LogModule.Info("TestUIDialog OnCreate");
        }

        public override void OnRefresh()
        {
            LogModule.Info("TestUIDialog OnRefresh");
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnDestroy()
        {
            LogModule.Info("TestUIDialog OnDestroy");
        }
    }