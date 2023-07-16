using YooAsset;

namespace MiniGame.Resource
{
    public class ResRemoteService : IRemoteServices
    {
        private readonly string _defaultHostServer;
        private readonly string _fallbackHostServer;
        
        public ResRemoteService(string defaultHostServer, string fallbackHostServer)
        {
            _defaultHostServer = defaultHostServer;
            _fallbackHostServer = fallbackHostServer;
        }
        
        public string GetRemoteMainURL(string fileName)
        {
            return $"{_defaultHostServer}/{fileName}";
        }

        public string GetRemoteFallbackURL(string fileName)
        {
            return $"{_fallbackHostServer}/{fileName}";
        }
    }
}