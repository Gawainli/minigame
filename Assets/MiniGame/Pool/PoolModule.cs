using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MiniGame.Logger;
using MiniGame.Module;
using UnityEngine;
using YooAsset;

namespace MiniGame.Pool
{
    public class PoolModuleCfg
    {
        public ResourcePackage pkg;
        public GameObject poolingRoot;
    }

    public class PoolModule : IModule
    {
        private static List<GameObjectPool> _gameObjectPools = new List<GameObjectPool>(100);
        private static List<GameObjectPool> _removeList = new List<GameObjectPool>(100);
        private static GameObject _spawnerRoot;
        private static ResourcePackage _package;

        public static string PackageName => _package.PackageName;

        public static GameObjectPool CreateGameObjectPoolSync(string assetPath, bool dontDestroy = false,
            int initCapacity = 0,
            int maxCapacity = int.MaxValue, float destroyTime = -1)
        {
            var pool = GetPool(assetPath);
            if (pool == null)
            {
                pool = new GameObjectPool(_spawnerRoot, assetPath, dontDestroy, initCapacity, maxCapacity, destroyTime);
                pool.CreatePoolSync(_package);
                _gameObjectPools.Add(pool);
            }

            return pool;
        }

        public static async UniTask<GameObjectPool> CreateGameObjectPoolAsync(string assetPath,
            bool dontDestroy = false,
            int initCapacity = 0,
            int maxCapacity = int.MaxValue, float destroyTime = -1)
        {
            var pool = GetPool(assetPath);
            if (pool == null)
            {
                pool = new GameObjectPool(_spawnerRoot, assetPath, dontDestroy, initCapacity, maxCapacity, destroyTime);
                await pool.CreatePoolAsync(_package);
                _gameObjectPools.Add(pool);
            }

            return pool;
        }

        public static GameObject Spawn(string assetPath)
        {
            return Spawn(assetPath, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Spawn(string assetPath, Vector3 position, Quaternion rotation, bool forceClone = false,
            Transform parent = null, params System.Object[] userData)
        {
            GameObjectPool pool = null;
            foreach (var p in _gameObjectPools)
            {
                if (p.AssetPath == assetPath)
                {
                    pool = p;
                    break;
                }
            }

            if (pool == null)
            {
                LogModule.Error($"PoolModule.Spawn: location {assetPath} not found");
                return null;
            }

            return pool.Spawn(parent, position, rotation, forceClone, userData);
        }

        public void DestroyAll(bool includeAll)
        {
            if (includeAll)
            {
                foreach (var pool in _gameObjectPools)
                {
                    pool.DestroyPool();
                }

                _gameObjectPools.Clear();
            }
            else
            {
                List<GameObjectPool> removeList = new List<GameObjectPool>();
                foreach (var pool in _gameObjectPools)
                {
                    if (pool.DontDestroy == false)
                        removeList.Add(pool);
                }

                foreach (var pool in removeList)
                {
                    _gameObjectPools.Remove(pool);
                    pool.DestroyPool();
                }
            }
        }

        public static GameObjectPool GetPool(string assetPath)
        {
            GameObjectPool pool = null;
            foreach (var p in _gameObjectPools)
            {
                if (p.AssetPath == assetPath)
                {
                    pool = p;
                    break;
                }
            }

            return pool;
        }


        #region IModule

        public void Initialize(object userData = null)
        {
            var cfg = userData as PoolModuleCfg;
            if (cfg == null)
            {
                LogModule.Error("PoolModule.Initialize: cfg is null");
                return;
            }

            _spawnerRoot = new GameObject($"{cfg.pkg.PackageName}");
            _spawnerRoot.transform.SetParent(cfg.poolingRoot.transform);
            _package = cfg.pkg;
            Initialized = true;
        }

        public void Tick(float deltaTime, float unscaledDeltaTime)
        {
            _removeList.Clear();
            foreach (var pool in _gameObjectPools)
            {
                if (pool.CanAutoDestroy())
                    _removeList.Add(pool);
            }

            foreach (var pool in _removeList)
            {
                _gameObjectPools.Remove(pool);
                pool.DestroyPool();
            }
        }

        public void Shutdown()
        {
            DestroyAll(true);
        }

        public int Priority { get; set; }
        public bool Initialized { get; set; }

        #endregion
    }
}