using System;
using System.Collections.Generic;
using MiniGame.Base;
using MiniGame.Logger;
using UnityEngine;

namespace MiniGame.Module
{
    public class ModuleCore : MonoBehaviour
    {
        private static readonly Dictionary<Type, IModule> ModulesDict = new Dictionary<Type, IModule>();

        public static T CreateModule<T>(int priority = 0, object userData = null) where T : class, IModule, new()
        {
            var type = typeof(T);
            if (ModulesDict.ContainsKey(type))
            {
                LogModule.Error($"Module is already created. {type.FullName}");
                return null;
            }

            var module = new T
            {
                Priority = priority
            };
            ModulesDict.Add(type, module);
            module.Initialize(userData);
            return module;
        }

        public static bool ContainsModule<T>() where T : class, IModule
        {
            var type = typeof(T);
            return ModulesDict.ContainsKey(type);
        }

        public static T GetModule<T>() where T : class, IModule
        {
            var type = typeof(T);
            if (ModulesDict.TryGetValue(type, out var value))
            {
                return value as T;
            }

            return null;
        }

        public static void RemoveModule<T>() where T : class, IModule
        {
            var type = typeof(T);
            if (ModulesDict.ContainsKey(type))
            {
                ModulesDict.Remove(type);
            }
        }

        #region Mono
        private void Update()
        {
            foreach (var module in ModulesDict.Values)
            {
                module.Tick(Time.deltaTime, Time.unscaledDeltaTime);
            }
        }

        private void OnDestroy()
        {
            foreach (var module in ModulesDict.Values)
            {
                module.Shutdown();
            }
        }
        #endregion
    }
}