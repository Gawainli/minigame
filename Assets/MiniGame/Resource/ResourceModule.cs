using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MiniGame.Base;
using MiniGame.Logger;
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
    
    public class ResourceModule : GameModule
    {
        public string packageName;
        public EPlayMode ePlayMode;
        public string hostURL;
        public string fallbackHostURL;

        private CancellationToken cancellationToken;
        
        public void InitYooAsset()
        {
            YooAssets.Initialize(new ResLogger());
            var pkg = YooAssets.TryGetPackage(packageName);
            if (pkg == null)
            {
                pkg = YooAssets.CreatePackage(packageName);
                YooAssets.SetDefaultPackage(pkg);
            }

            cancellationToken = this.GetCancellationTokenOnDestroy();
        }
    }
}