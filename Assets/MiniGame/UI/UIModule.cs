using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MiniGame.Logger;
using MiniGame.Module;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

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

        public static async UniTask<T> OpenWindowAsync<T>(string assetPath, int layer = 0, bool fullscreen = false, params System.Object[] userDatas)
            where T : UIWindow
        {
            var windowName = typeof(T).FullName;
            if (ContainsWindow(windowName))
            {
                var window = GetWindow(windowName);
                PopWindow(window);
                PushWindow(window);
                window.Create();
                window.Refresh(userDatas);
                return (T)window;
            }
            else
            {
                var window = Activator.CreateInstance<T>() as UIWindow;
                window.Init(windowName, layer, fullscreen);
                PushWindow(window);
                await window.LoadAsync(assetPath, userDatas);
                window.Create();
                window.Refresh(userDatas);
                return (T)window;
            }
        }
        
        public static T OpenWindowSync<T>(string assetPath, int layer = 0, bool fullscreen = false, params System.Object[] userDatas)
            where T : UIWindow
        {
            var windowName = typeof(T).FullName;
            if (ContainsWindow(windowName))
            {
                var window = GetWindow(windowName);
                PopWindow(window);
                PushWindow(window);
                window.Create();
                window.Refresh(userDatas);
                return (T)window;
            }
            else
            {
                var window = Activator.CreateInstance<T>() as UIWindow;
                window.Init(windowName, layer, fullscreen);
                PushWindow(window);
                window.LoadSync(assetPath, userDatas);
                window.Create();
                window.Refresh(userDatas);
                return (T)window;
            }
        }
        
        public static void CloseWindow(Type type)
        {
            CloseWindow(type.FullName);
        }
        
        public static void CloseWindow<T>() where T : UIWindow
        {
            CloseWindow(typeof(T).FullName);
        }
        
        public static void CloseWindow(string windowName)
        {
            if (ContainsWindow(windowName))
            {
                var window = GetWindow(windowName);
                window.Destroy();
                PopWindow(window);
                SortWindowDepth(window.WindowLayer);
                SetWindowVisible();
            }
        }

        public static void CloseAll()
        {
            for (int i = 0; i < WindowStack.Count; i++)
            {
                WindowStack[i].Destroy();
            }
            WindowStack.Clear();
        }

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
        
        public static bool ContainsWindow(string name)
        {
            foreach(var ui in WindowStack)
            {
                if (ui.WindowName == name)
                {
                    return true;
                }
            }

            return false;
        }
        
        public static UIWindow GetWindow(string name)
        {
            foreach(var ui in WindowStack)
            {
                if (ui.WindowName == name)
                {
                    return ui;
                }
            }

            return null;
        }
        
        private static void SetWindowVisible()
        {
            bool isHideNext = false;
            for (int i = WindowStack.Count - 1; i >= 0; i--)
            {
                UIWindow window = WindowStack[i];
                if (isHideNext == false)
                {
                    window.Visible = true;
                    if (window.Prepared && window.FullScreen)
                        isHideNext = true;
                }
                else
                {
                    window.Visible = false;
                }
            }
        }
        
        private static void SortWindowDepth(int layer)
        {
            int depth = layer;
            for (int i = 0; i < WindowStack.Count; i++)
            {
                if (WindowStack[i].WindowLayer == layer)
                {
                    WindowStack[i].Depth = depth;
                    depth += 100; //注意：每次递增100深度
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