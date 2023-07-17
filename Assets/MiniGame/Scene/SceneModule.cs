using System.Collections.Generic;
using MiniGame.Module;

namespace MiniGame.Scene
{
    public class SceneModule : IModule
    {
        public static Stack<UnityEngine.SceneManagement.Scene> SceneStack = new Stack<UnityEngine.SceneManagement.Scene>();
        
        
        public void Initialize(object userData = null)
        {
        }

        public void Tick(float deltaTime, float unscaledDeltaTime)
        {
        }

        public void Shutdown()
        {
        }

        public int Priority { get; set; }
    }
}