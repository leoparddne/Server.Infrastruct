using Common.Toolkit.Helper;
using System.Diagnostics;
using System.Reflection;

namespace Server.Infrastruct.WebAPI.BuilderExtension
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
                    assebly = new StackTrace().GetFrames().Last().GetMethod().Module.Assembly;
                }

                var debugableAttribute = assebly.GetCustomAttribute<DebuggableAttribute>();
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
