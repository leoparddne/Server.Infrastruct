using Common.Toolkit.Helper;
using Consul;
using Server.Infrastruct.WebAPI.Helper;

namespace Server.Infrastruct.WebAPI.BuilderExtension
{
    public static class ConsulBuilderExtension
    {
        private static string consulIP = AppSettingsHelper.GetSetting("Publish", "ConsulIP");
        private static int consulPort = AppSettingsHelper.GetSetting("Publish", "ConsulPort").ToInt();
        private static string applicationIP = AppSettingsHelper.GetSetting("Publish", "ApplicationIP");
        private static int applicationPort = AppSettingsHelper.GetSetting("Publish", "ApplicationPort").ToInt();
        private static string serviceName = AppSettingsHelper.GetSetting("Publish", "ConsulName");

        private static ConsulClient client = null;

        static ConsulBuilderExtension()
        {
            client = new ConsulClient(c =>
            {
                c.Address = new Uri($"http://{consulIP}:{consulPort}");
                c.Datacenter = "dc1";
                c.WaitTime = new TimeSpan(0, 0, 0, 30);
            });
        }


        /// <summary>
        /// Consul注册
        /// </summary>
        /// <param name="app"></param>
        public static void UseConsulRegist(this IApplicationBuilder app)
        {
            if (DebugExtension.ISDebug)
            {
                //LOG
                LogHelper.Warning($"{applicationIP}:{applicationPort} 启用debug模式");
                return;
            }

            KeepAlive();
        }

        /// <summary>
        /// consul掉线注册
        /// </summary>
        private static void KeepAlive()
        {
            Register();

            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        //不存在服务时候重新注册
                        while (!CheckAlive())
                        {
                            LogHelper.Info($"{serviceName}掉线,尝试重新注册consul:{applicationIP}{applicationPort}");

                            Register();
                            Thread.Sleep(5 * 1000);
                        }

                        LogHelper.Info($"{serviceName}consul通信检测正常:{applicationIP}{applicationPort}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }


                    //Thread.Sleep(1 * 10 * 1000);
                    Thread.Sleep(1 * 60 * 1000);
                }

            });
        }

        /// <summary>
        /// 存活状态检测
        /// </summary>
        /// <returns></returns>
        public static bool CheckAlive()
        {
            Task<QueryResult<Dictionary<string, AgentCheck>>> agent = client.Agent.Checks();
            Task<QueryResult<Dictionary<string, AgentService>>> services = client.Agent.Services();

            agent.Wait();
            services.Wait();

            //不存在服务时候重新注册
            return services.Result.Response.Values.Any(f => f.Service == serviceName && f.Address == applicationIP && f.Port == applicationPort);
        }

        private static ConsulClient Register()
        {

            int interval = AppSettingsHelper.GetSetting("ConsulSetting", "AgentServiceCheckInterval").ToInt();
            int timeout = AppSettingsHelper.GetSetting("ConsulSetting", "AgentServiceCheckTimeout").ToInt();
            int deregister = AppSettingsHelper.GetSetting("ConsulSetting", "AgentServiceCheckDeregister").ToInt();

            var data = client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = $"{serviceName}-{applicationIP}:{applicationPort}",
                Name = serviceName,
                Address = applicationIP,
                Port = applicationPort,
                Tags = new string[] { serviceName, $"{applicationIP}:{applicationPort}" },
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(interval),
                    HTTP = $"http://{applicationIP}:{applicationPort}/Api/Health/Index",
                    Timeout = TimeSpan.FromSeconds(timeout),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(deregister),
                }
            });

            data.Wait();

            LogHelper.Info($"{serviceName}注册consul:{applicationIP}{applicationPort}");
            return client;
        }


        public static string GetKV(string key)
        {
            string configValue = client.GetKV(key);

            return configValue;
        }
    }
}
