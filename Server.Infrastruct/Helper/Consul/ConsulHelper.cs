using Common.Toolkit.Helper;
using Consul;
using System.Text;

namespace Server.Infrastruct.Helper.Consul
{
    public static class ConsulHelper
    {
        public static string GetKV(this ConsulClient client, string key)
        {
            Task<QueryResult<KVPair>> config = client.KV.Get(key);
            config.Wait();
            QueryResult<KVPair> kvPair = config.Result;
            if (kvPair.Response != null && kvPair.Response.Value != null)
            {
                return Encoding.UTF8.GetString(kvPair.Response.Value);
                //return Encoding.UTF8.GetString(kvPair.Response.Value, 0, kvPair.Response.Value.Length);
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取已注册的服务
        /// </summary>
        /// <param name="client"></param>
        public static QueryResult<Dictionary<string, AgentService>> GetServices(this ConsulClient client)
        {
            Task<QueryResult<Dictionary<string, AgentService>>> sewrvice = client.Agent.Services();
            sewrvice.Wait();
            QueryResult<Dictionary<string, AgentService>> result = sewrvice.Result;
            return result;
        }

        /// <summary>
        /// 根据服务名和待绑定的ip获取随机端口
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ip"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static int GetConsulUnUsedPort(this ConsulClient client, string ip, string serviceName)
        {
            var service = client.GetServices();

            var allService = service.Response.Values;
            int port = 0;
            bool getSuccess = false;
            while (!getSuccess)
            {
                port = PortHelper.GetRandomUnusedPort();
                if (allService.Any(f => f.Address == ip))
                {
                    if (allService.Any(f => f.Service != serviceName && f.Port == port))
                    {
                        Thread.Sleep(300);
                        continue;
                    }

                }

                getSuccess = true;
            }

            return port;
        }
    }
}
