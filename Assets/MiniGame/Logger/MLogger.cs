using MiniGame.Base;

namespace MiniGame.Logger
{
    public static class MLogger
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
        
        public static void Assert(bool con, string msg)
        {
            if (MiniGameCore.ContainsModule<LogModule>())
            {
                MiniGameCore.GetModule<LogModule>().Assert(con, msg);
            }
            else
            {
                UnityEngine.Debug.Assert(con, msg);
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
        
        public static void Exception(string msg)
        {
            if (MiniGameCore.ContainsModule<LogModule>())
            {
                MiniGameCore.GetModule<LogModule>().Exception(msg);
            }
            else
            {
                UnityEngine.Debug.LogError(msg);
            }
        }
    }
}