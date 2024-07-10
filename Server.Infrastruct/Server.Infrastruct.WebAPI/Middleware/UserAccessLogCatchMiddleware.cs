using Common.Toolkit.Helper;
using Newtonsoft.Json;
using Server.Domain.Models.Model;
using System.Diagnostics;
using System.Text;
using System.Web;

namespace Server.Infrastruct.WebAPI.Middleware
{
    public class UserAccessLogCatchMiddleware
    {
        private readonly RequestDelegate next;

        private Stopwatch stopwatch;

        public UserAccessLogCatchMiddleware(RequestDelegate Next)
        {
            next = Next;
            stopwatch = new Stopwatch();
        }

        /// <summary>
        /// API请求日志抓取
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            bool isRecordLog = AppSettingsHelper.GetSetting("LogSetting", "Enabled").ToBool();
            string ignoreApis = AppSettingsHelper.GetSetting("LogSetting", "IgnoreApis");
            if (isRecordLog)
            {
                HttpRequest httpRequest = httpContext.Request;
                string api = httpRequest.Path.ToString().TrimEnd('/').ToLower();
                string ip = httpContext.Request.Headers["X-Forwarded-For"].ToString();
                if (string.IsNullOrEmpty(ip))
                {
                    ip = httpContext.Connection.RemoteIpAddress.ToString();
                }
                if (api.Contains("api") && !api.Contains("health") && !ignoreApis.Contains(api))
                {
                    stopwatch.Restart();

                    UserAccessLogModel userAccessLogModel = new UserAccessLogModel();

                    userAccessLogModel.IP = ip;
                    userAccessLogModel.API = api;
                    userAccessLogModel.BeginTime = DateTime.Now;
                    userAccessLogModel.RequetMethod = httpRequest.Method;
                    userAccessLogModel.Agent = httpRequest.Headers["User-Agent"].ToString();

                    if (httpRequest.Method.ToLower().Equals("post"))
                    {
                        httpRequest.EnableBuffering();


                        //Stream stream = httpRequest.Body;
                        //if (httpRequest.ContentLength != null)
                        //{
                        //    byte[] buffer = new byte[httpRequest.ContentLength.Value];
                        //    stream.Read(buffer, 0, buffer.Length);

                        //    userAccessLogModel.RequestData = Encoding.UTF8.GetString(buffer).Replace("\n", "");
                        //}


                        try
                        {
                            var reader = new StreamReader(httpRequest.BodyReader.AsStream(), Encoding.UTF8);

                            var data = reader.ReadToEnd();
                            userAccessLogModel.RequestData = data;
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex.Message);
                        }



                        httpRequest.Body.Position = 0;
                    }
                    else if (httpRequest.Method.ToLower().Equals("get"))
                    {
                        userAccessLogModel.RequestData = HttpUtility.UrlDecode(httpRequest.QueryString.ToString(), Encoding.UTF8);
                    }



                    httpContext.Response.OnCompleted(() =>
                    {
                        stopwatch.Stop();

                        userAccessLogModel.ExecuteTime = stopwatch.ElapsedMilliseconds + "ms";
                        string logInfo = JsonConvert.SerializeObject(userAccessLogModel);
                        Task.Run(() =>
                        {
                            LogHelper.WriteLog("UserAccess", new string[] { logInfo + "," });
                        });
                        return Task.CompletedTask;
                    });



                    var originalBodyStream = httpContext.Response.Body;
                    var responseBody = new MemoryStream();
                    //using (var responseBody = new MemoryStream())
                    //{
                    httpContext.Response.Body = responseBody;


                    await next(httpContext);


                    //重置读取位置 - 获取返回数据
                    responseBody.Seek(0, SeekOrigin.Begin);
                    var text = await new StreamReader(responseBody).ReadToEndAsync();

                    //重置读取位置 - 复制数据到原始流对象中
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                    httpContext.Response.Body = originalBodyStream;

                    userAccessLogModel.ResponseData = text;
                    //}
                }
                else
                {
                    await next(httpContext);
                }
            }
            else
            {
                await next(httpContext);
            }
        }
    }
}
