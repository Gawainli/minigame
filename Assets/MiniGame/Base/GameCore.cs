using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame.Base
{
    public class GameCore : MonoBehaviour
    {
        private static readonly Dictionary<Type, GameModule> _modules = new Dictionary<Type, GameModule>();

        public static T GetModule<T>() where T : GameModule
        {
            var type = typeof(T);
            if (_modules.ContainsKey(type))
            {
                return _modules[type] as T;
            }

            return null;
        }

        public static void RegisterModule<T>(T module) where T : GameModule
        {
            if (module == null)
            {
                return;
            }

            var type = typeof(T);
            if (_modules.ContainsKey(type))
            {
                return;
            }

            _modules.Add(type, module);
        }
    }
}