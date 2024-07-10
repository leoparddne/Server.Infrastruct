namespace Server.Domain.Models.Model.Job
{
    /// <summary>
    /// 脚本配置
    /// </summary>
    public class JobConfig
    {
        /// <summary>
        /// 基础路径
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// 基础路径下的具体服务
        /// </summary>
        public List<JobConfigItem> Jobs { get; set; }
    }
}
