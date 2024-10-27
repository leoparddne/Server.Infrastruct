using Common.Toolkit.Helper;
using System.Diagnostics;
using System.Reflection;

namespace Server.Infrastruct.Extension
{
    public static class DebugExtension
    {
        public static bool ISDebug { get; set; }
        public static bool RunningModeIsDebug
        {
            get
            {
                var assebly = Assembly.GetEntryAssembly();
                if (assebly == null)
                {
                    assebly = new StackTrace().GetFrames()?.Last()?.GetMethod()?.Module?.Assembly;
                }

                if (assebly == null)
                {
                    throw new Exception("can not catch assebly");
                }

                var debugableAttribute = assebly.GetCustomAttribute<DebuggableAttribute>();
                if (debugableAttribute == null)
                {
                    throw new Exception("can not catch debugableAttribute");
                }
                var isdebug = debugableAttribute.DebuggingFlags.HasFlag(DebuggableAttribute.DebuggingModes.EnableEditAndContinue);

                return isdebug;
            }
        }

        /// <summary>
        /// 启用调试模式
        /// 此方法仅在DEBUG模式下生效
        /// </summary>
        public static void UseDebug()
        {
            if (!RunningModeIsDebug)
            {
                Debug.WriteLine("请勿在非DEBUG模式调用UseDebug");
                LogHelper.Error("请勿在非DEBUG模式调用UseDebug");
                return;
            }
            ISDebug = true;
        }
    }
}
