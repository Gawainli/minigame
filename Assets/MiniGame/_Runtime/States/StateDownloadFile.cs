using Cysharp.Threading.Tasks;
using MiniGame.Asset;
using MiniGame.Logger;
using MiniGame.StateMachine;
using YooAsset;

namespace MiniGame.Runtime
{
    public class StateDownloadFile : State
    {
        public override void Init()
        {
        }

        public override async void Enter()
        {
            AssetModule.downloaderOperation.OnDownloadErrorCallback = OnDownloadErrorCallback;
            AssetModule.downloaderOperation.OnDownloadProgressCallback = OnDownloadProgress;
            await AssetModule.downloaderOperation.ToUniTask();
            if (AssetModule.downloaderOperation.Status == EOperationStatus.Succeed)
            {
                ChangeState<StatePatchDone>();
            }
        }

        public override void Exit()
        {
        }
        
        public void OnDownloadErrorCallback(string filename, string error)
        {
            LogModule.Error($"download error: {filename}, {error}");
        }

        public void OnDownloadProgress(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes,
            long currentDownloadBytes)
        {
            LogModule.Info($" download progress: {currentDownloadCount}/{totalDownloadCount}, {currentDownloadBytes}/{totalDownloadBytes}");
        }
            
    }
}
