using Common.Toolkit.Helper;
using Server.Domain.Models.Model;
using Server.Infrastruct.WebAPI.Model;

namespace Server.Infrastruct.WebAPI.ServiceExtension.Agent
{
    public class RawHttpAgentService : IRawHttpAgentService
    {
        protected Dictionary<string, string> header;
        protected string baseUrl;
        //默认baseurl为网关
        protected bool baseUrlIsFromGateWay = true;

        public int TimeOutSecond { get; set; } = 10;

        public RawHttpAgentService()
        {
        }


        public void SetTimeout(int seconds)
        {
            TimeOutSecond = seconds;
        }


        /// <summary>
        /// 设置baseurl-如果不设置则默认取配置中的网关地址
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public IRawHttpAgentService SetBaseUrl(string baseUrl)
        {
            this.baseUrl = baseUrl;
            baseUrlIsFromGateWay = false;
            return this;
        }


        public string GetUrl(string url)
        {
            //获取网关地址
            if (baseUrlIsFromGateWay && baseUrl != DBConfigSingleton.GetConfig().GateWay)
            {
                baseUrl = DBConfigSingleton.GetConfig().GateWay;
            }

            if (baseUrl.EndsWith('/'))
            {
                baseUrl = baseUrl.TrimEnd('/');
            }
            if (url.StartsWith('/'))
            {
                url = url.TrimStart('/');
            }

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return $"{url}";
            }
            else
            {
                return $"{baseUrl}/{url}";
            }
        }




        public T PostRaw<T>(string url, object obj = null, Dictionary<string, string> Headers = null)
        {
            var result = PostRawAsync<T>(url, obj, Headers);
            result.Wait();
            return result.Result;
        }

        public async Task<T> PostRawAsync<T>(string url, object obj = null, Dictionary<string, string> Headers = null)
        {
            var data = await HttpHelper.PostAsync<T>(GetUrl(url), obj, Headers ?? header, TimeOutSecond * 1000);
            return data;
        }
        public async Task<string> PostRawAsync(string url, object obj = null, Dictionary<string, string> Headers = null)
        {
            var data = await HttpHelper.PostAsync(GetUrl(url), obj, Headers ?? header, TimeOutSecond * 1000);
            return data;
        }

        public async Task<string> GetRawAsync(string url, Dictionary<string, object> obj = null, Dictionary<string, string> Headers = null)
        {
            var data = await HttpHelper.GetAsync(GetUrl(url), obj, Headers ?? header, TimeOutSecond * 1000);
            return data;
        }

        public async Task<T> GetRawAsync<T>(string url, Dictionary<string, object> obj = null, Dictionary<string, string> Headers = null)
        {
            var data = await HttpHelper.GetAsync<T>(GetUrl(url), obj, Headers ?? header, TimeOutSecond * 1000);
            return data;
        }

        public T GetRaw<T>(string url, Dictionary<string, object> obj = null, Dictionary<string, string> Headers = null)
        {
            var result = GetRawAsync<T>(url, obj, Headers);
            result.Wait();
            return result.Result;
        }


        public async Task<T> PostFormDataAsync<T>(string url, Dictionary<string, object> parameters = null, Dictionary<string, string> Headers = null)
        {
            var data = await HttpHelper.PostFormDataAsync<APIResponseModel<T>>(GetUrl(url), Headers ?? header, parameters, TimeOutSecond * 1000);
            return data.Data;
        }


        public T PostFormData<T>(string url, Dictionary<string, object> parameters = null, Dictionary<string, string> Headers = null)
        {
            var result = PostFormDataAsync<T>(url, parameters, Headers);
            result.Wait();
            return result.Result;
        }
    }
}
