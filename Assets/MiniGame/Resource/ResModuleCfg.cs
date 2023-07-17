using YooAsset;

namespace MiniGame.Resource
{
    public class ResModuleCfg
    {
        public string packageName;
        public EPlayMode ePlayMode;
        public string hostServerIP;
        public string appVersion;
        public string DefaultHostServer => GetHostServerURL();
        
        private string GetHostServerURL()
        {
            //string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
            if (string.IsNullOrEmpty(this.hostServerIP) || string.IsNullOrEmpty(this.appVersion))
            {
                hostServerIP = "http://127.0.0.1";
                appVersion = "v1.0";
            }

#if UNITY_EDITOR
            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
                return $"{hostServerIP}/CDN/Android/{appVersion}";
            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
                return $"{hostServerIP}/CDN/IPhone/{appVersion}";
            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
                return $"{hostServerIP}/CDN/WebGL/{appVersion}";
            else
                return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
		// if (Application.platform == RuntimePlatform.Android)
		// 	return $"{hostServerIP}/CDN/Android/{appVersion}";
		// else if (Application.platform == RuntimePlatform.IPhonePlayer)
		// 	return $"{hostServerIP}/CDN/IPhone/{appVersion}";
		// else if (Application.platform == RuntimePlatform.WebGLPlayer)
		// 	return $"{hostServerIP}/CDN/WebGL/{appVersion}";
		// else
		// 	return $"{hostServerIP}/CDN/PC/{appVersion}";
            return "";
#endif
        }
    }
}