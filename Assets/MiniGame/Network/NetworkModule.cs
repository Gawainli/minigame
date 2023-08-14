using MiniGame.Module;

namespace MiniGame.Network
{
    public class NetworkModule : IModule
    {
        public void Initialize(object userData = null)
        {
            throw new System.NotImplementedException();
        }

        public void Tick(float deltaTime, float unscaledDeltaTime)
        {
            throw new System.NotImplementedException();
        }

        public void Shutdown()
        {
            throw new System.NotImplementedException();
        }

        public int Priority { get; set; }
        public bool Initialized { get; set; }
    }
}