namespace Server.Domain.Models.Model
{
    public class UserAccessLogModel
    {
        /// <summary>
        /// 发起请求IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// API地址
        /// </summary>
        public string API { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// API花费时间(ms)
        /// </summary>
        public string ExecuteTime { get; set; }

        /// <summary>
        /// 请求方式POST/GET
        /// </summary>
        public string RequetMethod { get; set; }

        /// <summary>
        /// 请求数据
        /// </summary>
        public string RequestData { get; set; }

        /// <summary>
        /// 返回值
        /// </summary>
        public string ResponseData { get; set; }

        /// <summary>
        /// 代理
        /// </summary>
        public string Agent { get; set; }
    }
}
