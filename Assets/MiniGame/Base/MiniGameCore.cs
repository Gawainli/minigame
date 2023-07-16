using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame.Base
{
    public class MiniGameCore : GameModule
    {
        private static readonly Dictionary<Type, GameModule> _modules = new Dictionary<Type, GameModule>();
        
        public static bool ContainsModule<T>() where T : GameModule
        {
            var type = typeof(T);
            return _modules.ContainsKey(type);
        }

        public static T GetModule<T>() where T : GameModule
        {
            var type = typeof(T);
            if (_modules.ContainsKey(type))
            {
                return _modules[type] as T;
            }
            return null;
        }

        public static void RegisterModule(Type type, GameModule module)
        {
            if (module == null)
            {
                return;
            }
            
            if (_modules.ContainsKey(type))
            {
                return;
            }

            _modules.Add(type, module);
        }
    }
}