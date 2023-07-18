using MiniGame.Logger;
using MiniGame.Module;
using UnityEngine;

namespace MiniGame.UI
{
    public class UIModule : IModule
    {
        
        
        #region IModule
        public void Initialize(object userData = null)
        {
            LogModule.Info("UIModule Initialize");
            Initialized = true;
        }

        public void Tick(float deltaTime, float unscaledDeltaTime)
        {
        }

        public void Shutdown()
        {
        }

        public int Priority { get; set; }
        public bool Initialized { get; set; }

        #endregion
    }
}