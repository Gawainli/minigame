using System;
using UnityEngine;

namespace MiniGame.Base
{
    public class GameModule : MonoBehaviour
    {
        public virtual void Tick(float deltaTime, float unscaledDeltaTime){}
        public virtual void Shutdown(){}

        protected virtual void Update()
        {
            Tick(Time.deltaTime, Time.unscaledDeltaTime);
        }

        protected virtual void OnDestroy()
        {
            Shutdown();
        }
    }
}