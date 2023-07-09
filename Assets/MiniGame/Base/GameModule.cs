using System;
using UnityEngine;

namespace MiniGame.Base
{
    public abstract class GameModule
    {
        public abstract void Update(float deltaTime, float unscaledDeltaTime);
        public abstract void Shutdown();
    }
}