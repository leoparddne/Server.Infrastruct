using Common.Toolkit.Helper;
using Server.Infrastruct.Model.Models.Enums;
using Server.Infrastruct.Model.Models.Model;
using Server.Infrastruct.Services.Authentication;

namespace Server.Infrastruct.Services.Agent
{
    public class HttpAgentService : RawHttpAgentService, IHttpAgentService, IRawHttpAgentService
    {
        private readonly IAuthenticationService authenticationService;

        public HttpAgentService(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// 设置baseurl-如果不设置则默认取配置中的网关地址
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public new HttpAgentService SetBaseUrl(string baseUrl)
        {
            this.baseUrl = baseUrl;
            baseUrlIsFromGateWay = false;
            return this;
        }

        /// <summary>
        /// 注入token到字典中
        /// </summary>
        /// <returns></returns>
        public HttpAgentService InjectToken()
        {
            if (header == null)
            {
                header = new Dictionary<string, string>();
            }

            if (header.ContainsKey("Authorization"))
            {
                header["Authorization"] = authenticationService.GetAuthorization();
            }
            else
            {
                header.Add("Authorization", authenticationService.GetAuthorization());
            }

            return this;
        }

        public HttpAgentService InjectToken(Dictionary<string, string> Header)
        {
            Header.Add("Authorization", authenticationService.GetAuthorization());
            return this;
        }

        public static void CheckResult<T>(APIResponseModel<T>? data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("未获取到调用接口数据");
            }
            ExceptionHelper.Check(data.Code != (int)ResponseEnum.Success, data.Message);
        }

        public async Task<T> PostAsync<T>(string url, object? obj = null, Dictionary<string, string>? Headers = null)
        {
            try
            {
                var data = await HttpHelper.PostAsync<APIResponseModel<T>>(GetUrl(url), obj, Headers ?? header, TimeOutSecond * 1000);
                CheckResult(data);
                return data!.Data;
            }
            catch (Exception e)
            {
                throw new Exception(@$"子服务访问异常，目标URL :{url} 异常：{e.Message}");
            }
        }


        public T Post<T>(string url, object? obj = null, Dictionary<string, string>? Headers = null)
        {
            var result = PostAsync<T>(url, obj, Headers);
            result.Wait();
            return result.Result;
        }


        public async Task<T> GetAsync<T>(string url, Dictionary<string, object>? obj = null, Dictionary<string, string>? Headers = null)
        {
            try
            {
                var data = await HttpHelper.GetAsync<APIResponseModel<T>>(GetUrl(url), obj, Headers ?? header, TimeOutSecond * 1000);
                CheckResult(data);
                return data!.Data;
            }
            catch (Exception e)
            {
                throw new Exception(@$"子服务访问异常，目标URL :{url} 异常：{e.Message}");
            }
        }

        public T Get<T>(string url, Dictionary<string, object>? obj = null, Dictionary<string, string>? Headers = null)
        {
            var result = GetAsync<T>(url, obj, Headers);
            result.Wait();
            return result.Result;
        }

    }
}
