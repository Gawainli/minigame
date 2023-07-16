using System;
using System.Diagnostics;
using System.Text;
using MiniGame.Base;
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
    
    public class Logger : GameModule
    {
        public LogLevel filterLevel = LogLevel.Info;
        public OutputType outputType = OutputType.Editor;
        public bool useCustomColor = false;
        public bool useSystemColor = true;
        
        private static StringBuilder stringBuilder = new StringBuilder();
        
        public void Info(string msg)
        {
            InternalLog(LogLevel.Info, msg);
        }
        
        public void Warning(string msg)
        {
            InternalLog(LogLevel.Warning, msg);
        }
        
        public void Error(string msg)
        {
            InternalLog(LogLevel.Error, msg);
        }
        
        private void InternalLog(LogLevel type, string logString)
        {
            if (outputType == OutputType.None)
            {
                return;
            }

            if (type < filterLevel)
            {
                return;
            }

            StringBuilder infoBuilder = GetFormatString(type, logString, useSystemColor);
            string logStr = infoBuilder.ToString();

            //获取C#堆栈,Warning以上级别日志才获取堆栈
            if (type == LogLevel.Error|| type == LogLevel.Warning|| type == LogLevel.Exception)
            {
                StackFrame[] stackFrames = new StackTrace().GetFrames();
                for (int i = 0; i < stackFrames.Length; i++)
                {
                    StackFrame frame = stackFrames[i];
                    string declaringTypeName = frame.GetMethod().DeclaringType.FullName;
                    string methodName = stackFrames[i].GetMethod().Name;

                    infoBuilder.AppendFormat("[{0}::{1}\n", declaringTypeName, methodName);
                }
            }

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
        }
        
        private StringBuilder GetFormatString(LogLevel logLevel, string logString, bool bColor)
        {
            stringBuilder.Clear();
            switch (logLevel)
            {
                case LogLevel.Success:
                    if (useCustomColor)
                    {
                        stringBuilder.AppendFormat("<color=#0099bc><b>[MiniGame] ► </b></color><color=#00FF18><b>[SUCCESSED] ► </b></color>[{0}] - <color=#{2}>{1}</color>",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString, ColorUtils.Green);
                    }
                    else
                    {
                        stringBuilder.AppendFormat(
                            bColor ? "<color=#0099bc><b>[TEngine] ► </b></color><color=gray><b>[INFO] ► </b></color>[{0}] - <color=#00FF18>{1}</color>" : "<color=#0099bc><b>[MiniGame] ► </b></color><color=#00FF18><b>[SUCCESSED] ► </b></color>[{0}] - {1}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                    }
                    break;
                case LogLevel.Info:
                    if (useCustomColor)
                    {
                        stringBuilder.AppendFormat("<color=#0099bc><b>[MiniGame] ► </b></color><color=gray><b>[INFO] ► </b></color>[{0}] - <color=#{2}>{1}</color>" , 
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString,ColorUtils.Black);
                    }
                    else
                    {
                        stringBuilder.AppendFormat(
                            bColor ? "<color=#0099bc><b>[MiniGame] ► </b></color><color=gray><b>[INFO] ► </b></color>[{0}] - <color=gray>{1}</color>" : "<color=#0099bc><b>[MiniGame] ► </b></color><color=gray><b>[INFO] ► </b></color>[{0}] - {1}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                    }
                    break;
                case LogLevel.Assert:
                    if (useCustomColor)
                    {
                        stringBuilder.AppendFormat("<color=#0099bc><b>[MiniGame] ► </b></color><color=#FF00BD><b>[ASSERT] ► </b></color>[{0}] - <color=#{2}>{1}</color>",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString,ColorUtils.Exception);
                    }
                    else
                    {
                        stringBuilder.AppendFormat(
                            bColor ? "<color=#0099bc><b>[MiniGame] ► </b></color><color=#FF00BD><b>[ASSERT] ► </b></color>[{0}] - <color=green>{1}</color>" : "<color=#0099bc><b>[MiniGame] ► </b></color><color=#FF00BD><b>[ASSERT] ► </b></color>[{0}] - {1}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                    }
                    break;
                case LogLevel.Warning:
                    if (useCustomColor)
                    {
                        stringBuilder.AppendFormat("<color=#0099bc><b>[MiniGame] ► </b></color><color=#FF9400><b>[WARNING] ► </b></color>[{0}] - <color=#{2}>{1}</color>",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString, ColorUtils.Orange);
                    }
                    else
                    {
                        stringBuilder.AppendFormat(
                            bColor
                                ? "<color=#0099bc><b>[MiniGame] ► </b></color><color=#FF9400><b>[WARNING] ► </b></color>[{0}] - <color=yellow>{1}</color>"
                                : "<color=#0099bc><b>[MiniGame] ► </b></color><color=#FF9400><b>[WARNING] ► </b></color>[{0}] - {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"),
                            logString);
                    }
                    break;
                case LogLevel.Error:
                    if (useCustomColor)
                    {
                        stringBuilder.AppendFormat("<color=red><b>[ERROR] ► </b></color><color=#FF9400><b>[WARNING] ► </b></color>[{0}] - <color=#{2}>{1}</color>",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString, ColorUtils.Red);
                    }
                    else
                    {
                        stringBuilder.AppendFormat(
                            bColor ? "<color=#0099bc><b>[MiniGame] ► </b></color><color=red><b>[ERROR] ► </b></color>[{0}] - <color=red>{1}</color>" : "<color=#0099bc><b>[MiniGame] ► </b></color><color=red><b>[ERROR] ► </b></color>[{0}] - {1}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                    }
                    break;
                case LogLevel.Exception:
                    if (useCustomColor)
                    {
                        stringBuilder.AppendFormat("<color=red><b>[ERROR] ► </b></color><color=red><b>[EXCEPTION] ► </b></color>[{0}] - <color=#{2}>{1}</color>",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString, ColorUtils.Exception);
                    }
                    else
                    {
                        stringBuilder.AppendFormat(
                            bColor
                                ? "<color=#0099bc><b>[MiniGame] ► </b></color><color=red><b>[EXCEPTION] ► </b></color>[{0}] - <color=red>{1}</color>"
                                : "<color=#0099bc><b>[MiniGame] ► </b></color><color=red><b>[EXCEPTION] ► </b></color>[{0}] - {1}",
                            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), logString);
                    }
                    break;
            }
            return stringBuilder;
        }
    }
}