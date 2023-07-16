using System;
using UnityEngine;

namespace MiniGame.Base
{
    public class GameModule : MonoBehaviour
    {
        protected virtual void Initialize()
        {
            MiniGameCore.RegisterModule(this.GetType(), this);
        }

        public virtual void Tick(float deltaTime, float unscaledDeltaTime)
        {
        }

        public virtual void Shutdown()
        {
        }


        #region Mono

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void Update()
        {
            Tick(Time.deltaTime, Time.unscaledDeltaTime);
        }

        protected virtual void OnDestroy()
        {
            Shutdown();
        }

        #endregion
    }
}