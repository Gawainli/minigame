using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MiniGame.Asset;
using MiniGame.Logger;
using UnityEngine;
using YooAsset;

namespace MiniGame.Pool
{
    public class GameObjectPool
    {
        private readonly GameObject _root;
        private GameObject _prefab;
        private readonly Queue<GameObject> _cacheObjects;
        private readonly bool _dontDestroy;
        private readonly int _initCapacity;
        private readonly int _maxCapacity;
        private readonly float _destroyTime;
        private float _lastRestoreRealTime = -1f;



        /// <summary>
        /// 资源定位地址
        /// </summary>
        public string AssetPath { private set; get; }

        /// <summary>
        /// 内部缓存总数
        /// </summary>
        public int CacheCount
        {
            get { return _cacheObjects.Count; }
        }

        /// <summary>
        /// 外部使用总数
        /// </summary>
        public int SpawnCount { private set; get; } = 0;

        /// <summary>
        /// 是否常驻不销毁
        /// </summary>
        public bool DontDestroy
        {
            get { return _dontDestroy; }
        }


        public GameObjectPool(GameObject poolingRoot, string assetPath, bool dontDestroy, int initCapacity,
            int maxCapacity, float destroyTime)
        {
            _root = new GameObject(assetPath);
            _root.transform.parent = poolingRoot.transform;
            AssetPath = assetPath;

            _dontDestroy = dontDestroy;
            _initCapacity = initCapacity;
            _maxCapacity = maxCapacity;
            _destroyTime = destroyTime;

            // 创建缓存池
            _cacheObjects = new Queue<GameObject>(initCapacity);
        }

        /// <summary>
        /// 创建对象池
        /// </summary>
        public void CreatePoolSync(ResourcePackage package)
        {
            // 加载游戏对象
            // AssetHandle = package.LoadAssetSync<GameObject>(Location);
            _prefab = AssetModule.LoadAssetSync<GameObject>(AssetPath);

            // 创建初始对象
            for (int i = 0; i < _initCapacity; i++)
            {
                var go = Object.Instantiate(_prefab, _root.transform);
                go.SetActive(false);
                _cacheObjects.Enqueue(go);
            }
        }

        public async UniTask CreatePoolAsync(ResourcePackage package)
        {
            if (package == null)
            {
                LogModule.Error("CreatePoolAsync package is null");
                return;
            }

            // 加载游戏对象
            _prefab = await AssetModule.LoadAssetAsync<GameObject>(AssetPath);


            // 创建初始对象
            for (int i = 0; i < _initCapacity; i++)
            {
                var go = Object.Instantiate(_prefab, _root.transform);
                go.SetActive(false);
                _cacheObjects.Enqueue(go);
            }
        }

        /// <summary>
        /// 销毁游戏对象池
        /// </summary>
        public void DestroyPool()
        {
            // 销毁游戏对象
            Object.Destroy(_prefab);
            Object.Destroy(_root);
            _cacheObjects.Clear();
            SpawnCount = 0;
        }

        /// <summary>
        /// 查询静默时间内是否可以销毁
        /// </summary>
        public bool CanAutoDestroy()
        {
            if (_dontDestroy)
                return false;
            if (_destroyTime < 0)
                return false;

            if (_lastRestoreRealTime > 0 && SpawnCount <= 0)
                return (Time.realtimeSinceStartup - _lastRestoreRealTime) > _destroyTime;
            else
                return false;
        }

        /// <summary>
        /// 回收
        /// </summary>
        public void Restore(GameObject poolObj)
        {
            SpawnCount--;
            if (SpawnCount <= 0)
                _lastRestoreRealTime = Time.realtimeSinceStartup;

            // 如果缓存池还未满员
            if (_cacheObjects.Count < _maxCapacity)
            {
                SetRestoreGameObject(poolObj);
                _cacheObjects.Enqueue(poolObj);
            }
            else
            {
                Object.Destroy(poolObj);
            }
        }

        /// <summary>
        /// 丢弃
        /// </summary>
        public void Discard(GameObject poolObj)
        {
            SpawnCount--;
            if (SpawnCount <= 0)
                _lastRestoreRealTime = Time.realtimeSinceStartup;
            GameObject.Destroy(poolObj);
        }

        /// <summary>
        /// 获取一个游戏对象
        /// </summary>
        public GameObject Spawn(Transform parent, Vector3 position, Quaternion rotation, bool forceClone,
            params System.Object[] userDates)
        {
            GameObject go = null;
            if (forceClone == false && _cacheObjects.Count > 0)
                go = _cacheObjects.Dequeue();
            else
                go = GameObject.Instantiate(_prefab, _root.transform);

            go.transform.position = position;
            go.transform.rotation = rotation;
            var poolObj = go.GetComponent<IPoolObj>();
            if (poolObj != null)
            {
                poolObj.Init(userDates);
            }
            SpawnCount++;
            return go;
        }
        
        private void SetRestoreGameObject(GameObject go)
        {
            if (go != null)
            {
                go.SetActive(false);
                go.transform.SetParent(_root.transform);
                go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                var poolObj = go.GetComponent<IPoolObj>();
                if (poolObj != null)
                {
                    poolObj.Reset();
                }
            }
        }
    }
}