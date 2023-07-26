﻿using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using MiniGame.Logger;
using MiniGame.Module;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace MiniGame.Asset
{
    public class ResLogger : YooAsset.ILogger
    {
        public void Log(string message)
        {
            LogModule.Info(message);
        }

        public void Warning(string message)
        {
            LogModule.Warning(message);
        }

        public void Error(string message)
        {
            LogModule.Error(message);
        }

        public void Exception(Exception exception)
        {
            LogModule.Exception(exception.ToString());
        }
    }

    public class AssetModule : ModuleBase<AssetModule>, IModule
    {
        public static AssetModuleCfg cfg;
        public static ResourcePackage pkg;
        public static string packageVersion;
        
        public static DownloaderOperation downloaderOperation;
        public static int downloadingMaxNum = 10;
        public static int failedTryAgain = 3;

        private class GameDecryptionServices : IDecryptionServices
        {
            public ulong LoadFromFileOffset(DecryptFileInfo fileInfo)
            {
                return 32;
            }

            public byte[] LoadFromMemory(DecryptFileInfo fileInfo)
            {
                throw new NotImplementedException();
            }

            public Stream LoadFromStream(DecryptFileInfo fileInfo)
            {
                var bundleStream = new FileStream(fileInfo.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return bundleStream;
            }

            public uint GetManagedReadBufferSize()
            {
                return 1024;
            }
        }

        public void Initialize(object userData = null)
        {
            if (userData == null)
            {
                LogModule.Error("ResourceModule Initialize failed");
                return;
            }

            cfg = userData as AssetModuleCfg;
            if (cfg == null || string.IsNullOrEmpty(cfg.packageName))
            {
                LogModule.Error("ResourceModule Initialize failed");
                return;
            }

            YooAssets.Initialize(new ResLogger());
            pkg = YooAssets.TryGetPackage(cfg.packageName);
            if (pkg == null)
            {
                pkg = YooAssets.CreatePackage(cfg.packageName);
                YooAssets.SetDefaultPackage(pkg);
            }
        }

        public void Tick(float deltaTime, float unscaledDeltaTime)
        {
        }

        public void Shutdown()
        {
        }

        public int Priority { get; set; }
        public bool Initialized { get; set; }

        public static async UniTask<bool> InitPkgAsync()
        {
            // 编辑器下的模拟模式
            InitializationOperation initializationOperation = null;
            if (cfg.ePlayMode == EPlayMode.EditorSimulateMode)
            {
                var createParameters = new EditorSimulateModeParameters();
                createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(cfg.packageName);
                initializationOperation = pkg.InitializeAsync(createParameters);
            }

            // 单机运行模式
            if (cfg.ePlayMode == EPlayMode.OfflinePlayMode)
            {
                var createParameters = new OfflinePlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                initializationOperation = pkg.InitializeAsync(createParameters);
            }

            // 联机运行模式
            if (cfg.ePlayMode == EPlayMode.HostPlayMode)
            {
                // string defaultHostServer = GetHostServerURL();
                // string fallbackHostServer = GetHostServerURL();
                var createParameters = new HostPlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                // createParameters.QueryServices = new GameQueryServices();
                createParameters.RemoteServices = new YooRemoteService(cfg.DefaultHostServer, cfg.DefaultHostServer);
                initializationOperation = pkg.InitializeAsync(createParameters);
            }

            if (initializationOperation == null)
            {
                LogModule.Error("ResourceModule Initialize failed");
                return false;
            }

            await initializationOperation.ToUniTask();
            if (initializationOperation.Status == EOperationStatus.Succeed)
            {
                LogModule.Info("AssetModule Initialize Succeed");
                Instance.Initialized = true;
                return true;
            }
            else
            {
                LogModule.Error($"{initializationOperation.Error}");
                return false;
            }
        }

        public static async UniTask<bool> GetStaticVersion()
        {
            var op = pkg.UpdatePackageVersionAsync();
            await op.ToUniTask();
            if (op.Status == EOperationStatus.Succeed)
            {
                LogModule.Info($"GetStaticVersion Succeed");
                packageVersion = op.PackageVersion;
                return true;
            }
            else
            {
                LogModule.Error($"{op.Error}");
                return false;
            }
        }

        public static async UniTask<bool> UpdateManifest()
        {
            var op = pkg.UpdatePackageManifestAsync(packageVersion, true);
            await op.ToUniTask();
            
            if (op.Status == EOperationStatus.Succeed)
            {
                LogModule.Info($"UpdateManifest Succeed");
                return true;
            }
            else
            {
                LogModule.Error($"{op.Error}");
                return false;
            }
        }

        public static bool CreateDownloader()
        {
            var downloader = YooAssets.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            var totalDownloadCount = downloader.TotalDownloadCount;
            var totalDownloadBytes = downloader.TotalDownloadBytes;

            if (downloader.TotalDownloadCount == 0)
            {
                LogModule.Info("No files need to download");
                return false;
            }
            else
            {
                LogModule.Info($"Found {totalDownloadCount} files need to download, total size is {totalDownloadBytes} bytes");
                downloaderOperation = downloader;
                return true;
            }
        }

        public static T LoadAssetSync<T>(string path) where T : UnityEngine.Object
        {
            var op = YooAssets.LoadAssetSync<T>(path);
            if (op.Status == EOperationStatus.Succeed)
            {
                return op.AssetObject as T;
            }
            else
            {
                LogModule.Error($"{op.LastError}");
                return null;
            }
        }

        public static async UniTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            var op = YooAssets.LoadAssetAsync<T>(path);
            await op.ToUniTask();
            if (op.Status == EOperationStatus.Succeed)
            {
                var asset = op.AssetObject as T;
                op.Release();
                return asset;
            }
            else
            {
                LogModule.Error($"{op.LastError}");
                return null;
            }
        }

        public static GameObject LoadGameObjectSync(string path, Transform transform = null)
        {
            var op = YooAssets.LoadAssetSync<GameObject>(path);
            if (op.Status == EOperationStatus.Succeed)
            {
                var go = op.InstantiateSync(transform);
                op.Release();
                return go;
            }
            else
            {
                LogModule.Error($"{op.LastError}");
                return null;
            }
        }

        public static async UniTask<GameObject> LoadGameObjectAsync(string path, Transform transform = null)
        {
            var op = YooAssets.LoadAssetAsync<GameObject>(path);
            await op.ToUniTask();
            if (op.Status == EOperationStatus.Succeed)
            {
                var go = op.InstantiateSync(transform);
                op.Release();
                return go;
            }
            else
            {
                LogModule.Error($"{op.LastError}");
                return null;
            }
        }

        public static async UniTask<AssetOperationHandle> LoadAssetAsyncOp<T>(string path,
            Action<AssetOperationHandle> callback = null) where T : UnityEngine.Object
        {
            var op = YooAssets.LoadAssetAsync<T>(path);
            await op.ToUniTask();
            if (op.Status == EOperationStatus.Succeed)
            {
                callback?.Invoke(op);
                return op;
            }
            else
            {
                LogModule.Error($"{op.LastError}");
                return null;
            }
        }

        public static async UniTask<UnityEngine.SceneManagement.Scene> LoadSceneAsync(string path,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            var op = YooAssets.LoadSceneAsync(path, loadSceneMode);
            await op.ToUniTask();
            if (op.Status == EOperationStatus.Succeed)
            {
                return op.SceneObject;
            }
            else
            {
                LogModule.Error($"{op.LastError}");
                return default;
            }
        }

        public static void UnloadUnusedAssets()
        {
            pkg.UnloadUnusedAssets();
            GC.Collect();
        }
    }
}