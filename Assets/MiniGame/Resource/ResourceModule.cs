using System;
using System.IO;
using Cysharp.Threading.Tasks;
using MiniGame.Logger;
using MiniGame.Module;
using YooAsset;

namespace MiniGame.Resource
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
    
    public class ResourceModule : ModuleBase<ResourceModule>, IModule
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
        
        public ResModuleCfg cfg;
        private ResourcePackage _pkg;
        public void Initialize(object userData = null)
        {
            if (userData == null)
            {
                LogModule.Error("ResourceModule Initialize failed");
                return;
            }
            
            cfg = userData as ResModuleCfg;
            if (cfg == null || string.IsNullOrEmpty(cfg.packageName))
            {
                LogModule.Error("ResourceModule Initialize failed");
                return;
            }
            
            YooAssets.Initialize(new ResLogger());
            _pkg = YooAssets.TryGetPackage(cfg.packageName);
            if (_pkg == null)
            {
                _pkg = YooAssets.CreatePackage(cfg.packageName);
                YooAssets.SetDefaultPackage(_pkg);
            }
        }

        public void Tick(float deltaTime, float unscaledDeltaTime)
        {
        }

        public void Shutdown()
        {
        }

        public int Priority { get; set; }

        public async void InitPkgAsync()
        {
            // 编辑器下的模拟模式
            InitializationOperation initializationOperation = null;
            if (cfg.ePlayMode == EPlayMode.EditorSimulateMode)
            {
                var createParameters = new EditorSimulateModeParameters();
                createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(cfg.packageName);
                initializationOperation = _pkg.InitializeAsync(createParameters);
            }
            // 单机运行模式
            if (cfg.ePlayMode == EPlayMode.OfflinePlayMode)
            {
                var createParameters = new OfflinePlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                initializationOperation = _pkg.InitializeAsync(createParameters);
            }

            // 联机运行模式
            if (cfg.ePlayMode == EPlayMode.HostPlayMode)
            {
                // string defaultHostServer = GetHostServerURL();
                // string fallbackHostServer = GetHostServerURL();
                var createParameters = new HostPlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                // createParameters.QueryServices = new GameQueryServices();
                createParameters.RemoteServices = new ResRemoteService(cfg.DefaultHostServer, cfg.DefaultHostServer);
                initializationOperation = _pkg.InitializeAsync(createParameters);
            }

            if (initializationOperation == null)
            {
                LogModule.Error("ResourceModule Initialize failed");
                return;
            }

            await initializationOperation.ToUniTask();
            if (initializationOperation.Status == EOperationStatus.Succeed)
            {
                LogModule.Info("ResourceModule Initialize Succeed");
            }
            else
            {
                LogModule.Error($"{initializationOperation.Error}");
            }
        }
    }
}