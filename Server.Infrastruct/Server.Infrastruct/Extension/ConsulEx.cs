using Common.Toolkit.Helper;
using Consul;
using Server.Infrastruct.Helper.Consul;

namespace Server.Infrastruct.Extension
{
    public static class ConsulEx
    {
        public static ConsulConfig ConsulConfig { get; }

        private static ConsulClient? client = null;

        static ConsulEx()
        {
            var config = AppSettingsHelper.GetObject<ConsulConfig>("Publish");
            if (config == null)
            {
                throw new Exception("consul config(ip/port) is null");
            }
            ConsulConfig = config;

            InitConsul();
        }

        public static void InitConsul()
        {
            client = new ConsulClient(c =>
            {
                c.Address = new Uri($"http://{ConsulConfig.ConsulIP}:{ConsulConfig.ConsulPort}");
                c.Datacenter = "dc1";
                c.WaitTime = new TimeSpan(0, 0, 0, 30);
            });
        }




        /// <summary>
        /// consul掉线注册
        /// </summary>
        public static void KeepAlive()
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
                            LogHelper.Info($"{ConsulConfig.ConsulName}掉线,尝试重新注册consul:{ConsulConfig.ApplicationIP}{ConsulConfig.ApplicationPort}");

                            Register();
                            Thread.Sleep(5 * 1000);
                        }

                        LogHelper.Info($"{ConsulConfig.ConsulName}consul通信检测正常:{ConsulConfig.ApplicationIP}{ConsulConfig.ApplicationPort}");
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
            if (client == null)
            {
                InitConsul();
            }


            Task<QueryResult<Dictionary<string, AgentCheck>>> agent = client!.Agent.Checks();
            Task<QueryResult<Dictionary<string, AgentService>>> services = client.Agent.Services();

            agent.Wait();
            services.Wait();

            //不存在服务时候重新注册
            return services.Result.Response.Values.Any(f => f.Service == ConsulConfig.ConsulName && f.Address == ConsulConfig.ApplicationIP && f.Port == ConsulConfig.ApplicationPort);
        }

        private static ConsulClient Register()
        {
            if (client == null)
            {
                InitConsul();
            }

            var consulSetting = AppSettingsHelper.GetObject<ConsulSetting>("ConsulSetting");
            if (consulSetting == null)
            {
                throw new Exception("consul setting (check interval) is null");
            }

            int interval = consulSetting.AgentServiceCheckInterval;
            int timeout = consulSetting.AgentServiceCheckTimeout;
            int deregister = consulSetting.AgentServiceCheckDeregister;

            var data = client!.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = $"{ConsulConfig.ConsulName}-{ConsulConfig.ApplicationIP}:{ConsulConfig.ApplicationPort}",
                Name = ConsulConfig.ConsulName,
                Address = ConsulConfig.ApplicationIP,
                Port = ConsulConfig.ApplicationPort,
                Tags = new string[] { ConsulConfig.ConsulName, $"{ConsulConfig.ApplicationIP}:{ConsulConfig.ApplicationPort}" },
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(interval),
                    HTTP = $"http://{ConsulConfig.ApplicationIP}:{ConsulConfig.ApplicationPort}/Api/Health/Index",
                    Timeout = TimeSpan.FromSeconds(timeout),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(deregister),
                }
            });

            data.Wait();

            LogHelper.Info($"{ConsulConfig.ConsulName}注册consul:{ConsulConfig.ApplicationIP}{ConsulConfig.ApplicationPort}");
            return client;
        }


        public static string GetKV(string key)
        {
            if (client == null)
            {
                InitConsul();
            }

            string configValue = client!.GetKV(key);

            return configValue;
        }
    }
}
