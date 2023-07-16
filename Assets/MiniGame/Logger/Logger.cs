using MiniGame.Base;

namespace MiniGame.Logger
{
    public static class Logger
    {
        public static void Info(string msg)
        {
            if (MiniGameCore.ContainsModule<LogModule>())
            {
                MiniGameCore.GetModule<LogModule>().Info(msg);
            }
            else
            {
                UnityEngine.Debug.Log(msg);
            }
        }
        
        public static void Warning(string msg)
        {
            if (MiniGameCore.ContainsModule<LogModule>())
            {
                MiniGameCore.GetModule<LogModule>().Warning(msg);
            }
            else
            {
                UnityEngine.Debug.LogWarning(msg);
            }
        }
        
        public static void Error(string msg)
        {
            if (MiniGameCore.ContainsModule<LogModule>())
            {
                MiniGameCore.GetModule<LogModule>().Error(msg);
            }
            else
            {
                UnityEngine.Debug.LogError(msg);
            }
        }
    }
}