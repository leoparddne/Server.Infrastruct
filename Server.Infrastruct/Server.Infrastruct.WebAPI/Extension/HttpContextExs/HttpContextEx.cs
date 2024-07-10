using Server.Domain.Models.Model;
using System.Text;
using System.Web;


namespace Server.Infrastruct.WebAPI.Extension.HttpContextExs
{
    public static class HttpContextEx
    {
        /// <summary>
        /// 获取访问基本信息
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static UserAccessLogModel GetAccessModel(HttpContext httpContext)
        {
            UserAccessLogModel userAccessLogModel = new UserAccessLogModel();
            HttpRequest httpRequest = httpContext.Request;
            string api = httpRequest.Path.ToString().TrimEnd('/').ToLower();
            string ip = httpContext.Request.Headers["X-Forwarded-For"].ToString();
            if (string.IsNullOrEmpty(ip))
            {
                ip = httpContext.Connection.RemoteIpAddress.ToString();
            }
            userAccessLogModel.IP = ip;
            userAccessLogModel.API = api;
            userAccessLogModel.BeginTime = DateTime.Now;
            userAccessLogModel.RequetMethod = httpRequest.Method;
            userAccessLogModel.Agent = httpRequest.Headers["User-Agent"].ToString();

            if (httpRequest.Method.ToLower().Equals("post"))
            {
                httpRequest.EnableBuffering();

                //重置
                httpRequest.Body.Seek(0, SeekOrigin.Begin);
                Stream stream = httpRequest.Body;
                byte[] buffer = new byte[httpRequest.ContentLength.Value];
                stream.Read(buffer, 0, buffer.Length);

                userAccessLogModel.RequestData = Encoding.UTF8.GetString(buffer).Replace("\n", "");

                httpRequest.Body.Position = 0;
            }
            else if (httpRequest.Method.ToLower().Equals("get"))
            {
                userAccessLogModel.RequestData = HttpUtility.UrlDecode(httpRequest.QueryString.ToString(), Encoding.UTF8);
            }

            return userAccessLogModel;
        }
    }
}
