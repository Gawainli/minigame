﻿using MiniGame.Logger;

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
}