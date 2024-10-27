using Common.Toolkit.Helper;
using Server.Infrastruct.Extension;

namespace Server.Infrastruct.WebAPI.BuilderExtension
{
    public static class ConsulEx
    {
        /// <summary>
        /// Consul注册
        /// </summary>
        /// <param name="app"></param>
        public static void UseConsulRegist(this IApplicationBuilder app)
        {
            if (DebugExtension.ISDebug)
            {
                //LOG
                LogHelper.Warning($"consul注册， 启用debug模式跳过注册");
                return;
            }

            Infrastruct.Extension.ConsulEx.KeepAlive();
        }
    }
}
