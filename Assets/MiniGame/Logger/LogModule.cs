using System;
using System.Diagnostics;
using System.Text;
using MiniGame.Base;
using MiniGame.Module;
using MiniGame.Utils;

namespace MiniGame.Logger
{
    public enum LogLevel
    {
        Info,
        Success,
        Assert,
        Warning,
        Error,
        Exception,
    }

    [System.Flags]
    public enum OutputType
    {
        None = 0,
        Editor = 0x1,
        Gui = 0x2,
        File = 0x4
    }

    public class LogModule : ModuleBase<LogModule>, IModule
    {
        public static LogLevel filterLevel = LogLevel.Info;
        public static OutputType outputType = OutputType.Editor;
        
        public delegate void OnLogFunc(LogLevel type, string msg);
        public static event OnLogFunc OnLog;

        private static StringBuilder stringBuilder = new StringBuilder(1024);
        
        public static void Info(string msg)
        {
            InternalLog(LogLevel.Info, msg);
        }
        
        public static void Assert(bool condition, string msg = "")
        {
            if (!condition)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    msg = string.Format("{0}", "Assert Failed");
                }
                InternalLog(LogLevel.Assert, msg);
            }
        }

        public static void Warning(string msg)
        {
            InternalLog(LogLevel.Warning, msg);
        }

        public static void Error(string msg)
        {
            InternalLog(LogLevel.Error, msg);
        }
        
        public static void Exception(string msg)
        {
            InternalLog(LogLevel.Exception, msg);
        }

        private static void InternalLog(LogLevel type, string logString)
        {
            if (outputType == OutputType.None)
            {
                return;
            }

            if (type < filterLevel)
            {
                return;
            }

            StringBuilder infoBuilder = GetFormatString(type, logString);

            //获取C#堆栈,Warning以上级别日志才获取堆栈
            if (type == LogLevel.Error || type == LogLevel.Warning || type == LogLevel.Exception)
            {
                infoBuilder.AppendFormat("\n");
                StackFrame[] stackFrames = new StackTrace().GetFrames();
                for (int i = 0; i < stackFrames.Length; i++)
                {
                    StackFrame frame = stackFrames[i];
                    string declaringTypeName = frame.GetMethod().DeclaringType.FullName;
                    string methodName = stackFrames[i].GetMethod().Name;

                    infoBuilder.AppendFormat("{0}::{1}\n", declaringTypeName, methodName);
                }
            }

            string logStr = infoBuilder.ToString();
            
            if (type == LogLevel.Info || type == LogLevel.Success)
            {
                UnityEngine.Debug.Log(logStr);
            }
            else if (type == LogLevel.Warning)
            {
                UnityEngine.Debug.LogWarning(logStr);
            }
            else if (type == LogLevel.Assert)
            {
                UnityEngine.Debug.LogAssertion(logStr);
            }
            else if (type == LogLevel.Error)
            {
                UnityEngine.Debug.LogError(logStr);
            }
            else if (type == LogLevel.Exception)
            {
                UnityEngine.Debug.LogError(logStr);
            }

            if (OnLog != null)
            {
                OnLog.Invoke(type, logStr);
            }
        }

        private static StringBuilder GetFormatString(LogLevel logLevel, string logString)
        {
            stringBuilder.Clear();
            switch (logLevel)
            {
                case LogLevel.Success:
                {
                    stringBuilder.AppendFormat(
                        "<color=#0099bc><b>[TEngine] ► </b></color><color=gray><b>[INFO] ► </b></color>[{0}] - <color=#00FF18>{1}</color>",
                        System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                }
                    break;
                case LogLevel.Info:
                {
                    stringBuilder.AppendFormat(
                        "<color=#0099bc><b>[MiniGame] ► </b></color><color=gray><b>[INFO] ► </b></color>[{0}] - <color=gray>{1}</color>",
                        System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                }
                    break;
                case LogLevel.Assert:
                {
                    stringBuilder.AppendFormat(
                        "<color=#0099bc><b>[MiniGame] ► </b></color><color=#FF00BD><b>[ASSERT] ► </b></color>[{0}] - <color=green>{1}</color>",
                        System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                }
                    break;
                case LogLevel.Warning:
                {
                    stringBuilder.AppendFormat(
                        "<color=#0099bc><b>[MiniGame] ► </b></color><color=#FF9400><b>[WARNING] ► </b></color>[{0}] - <color=yellow>{1}</color>",
                        System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                }
                    break;
                case LogLevel.Error:
                {
                    stringBuilder.AppendFormat(
                        "<color=#0099bc><b>[MiniGame] ► </b></color><color=red><b>[ERROR] ► </b></color>[{0}] - <color=red>{1}</color>",
                        System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                }
                    break;
                case LogLevel.Exception:
                {
                    stringBuilder.AppendFormat(
                        "<color=#0099bc><b>[MiniGame] ► </b></color><color=red><b>[EXCEPTION] ► </b></color>[{0}] - <color=red>{1}</color>",
                        System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                }
                    break;
            }

            return stringBuilder;
        }

        public void Initialize(object userData = null)
        {
            Info("LogModule Initialize");
            Initialized = true;
        }

        public void Tick(float deltaTime, float unscaledDeltaTime)
        {
        }

        public void Shutdown()
        {
        }

        public int Priority { get; set; }
        public bool Initialized { get; set; }
    }
}