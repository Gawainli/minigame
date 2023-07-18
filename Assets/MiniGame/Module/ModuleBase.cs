using MiniGame.Logger;

namespace MiniGame.Module
{
    public interface IModule
    {
        void Initialize(object userData = null);

        void Tick(float deltaTime, float unscaledDeltaTime);

        void Shutdown();
        
        int Priority { get; set; }
        
        bool Initialized { get; set; }
    }

    public class ModuleBase<T> where T : class, IModule
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    LogModule.Error($"Module is not created. {typeof(T).FullName}");
                }
                return _instance;
            }
        }

        protected ModuleBase()
        {
            _instance = this as T;
        }

    }
}