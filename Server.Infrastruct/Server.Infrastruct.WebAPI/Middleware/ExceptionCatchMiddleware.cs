using Common.Toolkit.ExceptionEx;
using Common.Toolkit.Helper;
using Newtonsoft.Json;
using Server.Domain.Dto.OutDto;
using Server.Domain.Models.Enums;
using Server.Domain.Models.ExceptionExtention;
using Server.Domain.Models.Model;
using Server.Infrastruct.WebAPI.Extension.HttpContextExs;
using System.Diagnostics;
using System.Text;

namespace Server.Infrastruct.WebAPI.Middleware
{
    public class ExceptionCatchMiddleware
    {
        private readonly RequestDelegate next;
        Stopwatch stopwatch;

        public ExceptionCatchMiddleware(RequestDelegate Next)
        {
            next = Next;
            stopwatch = new Stopwatch();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            stopwatch.Restart();


            var originalBodyStream = httpContext.Response.Body;
            var responseBody = new MemoryStream();
            //using (var responseBody = new MemoryStream())
            //{
            httpContext.Response.Body = responseBody;

            Exception execErr = null;

            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                execErr = ex;
            }


            //重置读取位置 - 获取返回数据
            responseBody.Seek(0, SeekOrigin.Begin);

            if (execErr != null)
            {
                var json = ExceptionCatchAsync(httpContext, execErr, stopwatch);

                responseBody.Write(json.ToBytes());

                //重置读取位置 - 复制数据到原始流对象中
                responseBody.Seek(0, SeekOrigin.Begin);
            }

            await responseBody.CopyToAsync(originalBodyStream);
            httpContext.Response.Body = originalBodyStream;
        }


        /// <summary>
        /// 全局异常捕获
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="exception"></param>
        /// <param name="stopwatch"></param>
        /// <returns></returns>
        private static string ExceptionCatchAsync(HttpContext httpContext, Exception exception, Stopwatch stopwatch)
        {
            APIResponseModel<object> result;
            switch (exception)
            {
                case TokenExpiredException:
                    result = new APIResponseModel<object>
                    {
                        Code = ResponseEnum.TimeExpired.GetHashCode(),
                        Message = "TimeExpired"
                    };
                    break;
                case UnAuthorizedException:
                    result = new APIResponseModel<object>
                    {
                        Code = ResponseEnum.Unauthorized.GetHashCode(),
                        Message = "Unauthorized"
                    };
                    break;
                case LangageException langageException:
                    result = new APIResponseModel<object>
                    {
                        Code = ResponseEnum.LangageInfo.GetHashCode(),
                        Data = new LangageExceptionOutDto(langageException.LangageKey, langageException.LangageValue)
                    };
                    break;
                case ImportException importException:
                    result = new APIResponseModel<object>
                    {
                        Code = ResponseEnum.ImportInfo.GetHashCode(),
                        Data = importException.DataList
                    };
                    break;
                case HttpCodeException httpCodeException:
                    result = new APIResponseModel<object>
                    {
                        Code = ResponseEnum.Fail.GetHashCode(),
                        Message = httpCodeException.Message
                    };
                    httpContext.Response.StatusCode = httpCodeException.HttpCode;
                    break;

                default:
                    result = new APIResponseModel<object>
                    {
                        Code = ResponseEnum.Fail.GetHashCode(),
                        Message = exception.Message
                    };
                    break;
            }
            httpContext.Response.ContentType = "application/json";

            string strJson = JsonConvert.SerializeObject(result);
            UserAccessLogModel userAccessLogModel = HttpContextEx.GetAccessModel(httpContext);

            httpContext.Response.OnCompleted(() =>
            {
                stopwatch.Stop();

                userAccessLogModel.ResponseData = strJson;
                userAccessLogModel.ExecuteTime = stopwatch.ElapsedMilliseconds + "ms";
                string logInfo = JsonConvert.SerializeObject(userAccessLogModel);

                var stringBuilder = new StringBuilder();
                stringBuilder.Append(logInfo);
                stringBuilder.Append(",");
                stringBuilder.Append(exception.Message);
                stringBuilder.Append(exception.StackTrace);
                stringBuilder.Append(",");

                LogHelper.WriteLog("Exception", new string[] { stringBuilder.ToString() });
                return Task.CompletedTask;
            });

            return strJson;
        }
    }
}