using System.Collections.Generic;
using MiniGame.Logger;
using MiniGame.Module;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGame.UI
{
    public class UIModuleCfg
    {
        public bool pixelPerfect = false;
        public bool matchWidthOrHeight = false;
        public float referenceResolutionX = 1920;
        public float referenceResolutionY = 1080;
        
        public bool ignoreReversedGraphics = false;
    }
    public class UIModule : IModule
    {
        internal static GameObject UIRoot;
        internal static readonly List<UIWindow> WindowStack = new List<UIWindow>();

        private GameObject NewUIRoot(UIModuleCfg cfg)
        {
            var root = new GameObject("UIRoot");
            var canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = cfg.pixelPerfect;
            var scaler = root.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.matchWidthOrHeight = cfg.matchWidthOrHeight? 0 : 1;
            scaler.referenceResolution = new Vector2(cfg.referenceResolutionX, cfg.referenceResolutionY);
            var raycaster = root.AddComponent<GraphicRaycaster>();
            raycaster.ignoreReversedGraphics = cfg.ignoreReversedGraphics;
            return root;
        }
        
        private static void PushWindow(UIWindow window)
        {
            if (window == null)
            {
                return;
            }

            var index = -1;
            for (int i = 0; i < WindowStack.Count; i++)
            {
                if (window.WindowLayer < WindowStack[i].WindowLayer)
                {
                    index = i + 1;
                }
            }

            if (index == -1)
            {
                for (int i = 0; i < WindowStack.Count; i++)
                {
                    if (window.WindowLayer > WindowStack[i].WindowLayer)
                    {
                        index = i + 1;
                    }
                }
            }

            if (index == -1)
            {
                index = 0;
            }
            
            WindowStack.Insert(index, window);
        }
        
        private static void PopWindow(UIWindow window)
        {
            WindowStack.Remove(window);
        }
        
        private static void ContainsWindow(UIWindow window)
        {
            foreach(var ui in WindowStack)
            {
                if (ui.WindowName == window.WindowName)
                {
                    return;
                }
            }
        }
        
        
        
        #region IModule
        public void Initialize(object userData = null)
        {
            LogModule.Info("UIModule Initialize");
            if (userData is UnityEngine.GameObject uiRoot)
            {
                UIRoot = uiRoot;
            }
            else if(userData is UIModuleCfg cfg)
            {
                UIRoot = NewUIRoot(cfg);
            }
            else if(userData == null)
            {
                UIRoot = NewUIRoot(new UIModuleCfg());
            }
            Initialized = true;
        }
        
        public void Tick(float deltaTime, float unscaledDeltaTime)
        {
            if (Initialized)
            {
                foreach (var ui in WindowStack)
                {
                    ui?.Update(deltaTime);
                }
            }
        }

        public void Shutdown()
        {
        }

        public int Priority { get; set; }
        public bool Initialized { get; set; }

        #endregion
    }
}