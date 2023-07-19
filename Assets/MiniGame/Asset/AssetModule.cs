using System;
using System.IO;
using Cysharp.Threading.Tasks;
using MiniGame.Logger;
using MiniGame.Module;
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
        
        public static AssetModuleCfg cfg;
        public static ResourcePackage Pkg;
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
            Pkg = YooAssets.TryGetPackage(cfg.packageName);
            if (Pkg == null)
            {
                Pkg = YooAssets.CreatePackage(cfg.packageName);
                YooAssets.SetDefaultPackage(Pkg);
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

        public static async UniTask InitPkgAsync()
        {
            // 编辑器下的模拟模式
            InitializationOperation initializationOperation = null;
            if (cfg.ePlayMode == EPlayMode.EditorSimulateMode)
            {
                var createParameters = new EditorSimulateModeParameters();
                createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(cfg.packageName);
                initializationOperation = Pkg.InitializeAsync(createParameters);
            }
            // 单机运行模式
            if (cfg.ePlayMode == EPlayMode.OfflinePlayMode)
            {
                var createParameters = new OfflinePlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                initializationOperation = Pkg.InitializeAsync(createParameters);
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
                initializationOperation = Pkg.InitializeAsync(createParameters);
            }

            if (initializationOperation == null)
            {
                LogModule.Error("ResourceModule Initialize failed");
                return;
            }

            await initializationOperation.ToUniTask();
            if (initializationOperation.Status == EOperationStatus.Succeed)
            {
                LogModule.Info("AssetModule Initialize Succeed");
                Instance.Initialized = true;
            }
            else
            {
                LogModule.Error($"{initializationOperation.Error}");
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
                return op.AssetObject as T;
            }
            else
            {
                LogModule.Error($"{op.LastError}");
                return null;
            }
        }
        
        public static async UniTask<UnityEngine.SceneManagement.Scene> LoadSceneAsync(string path, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
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
    }
}