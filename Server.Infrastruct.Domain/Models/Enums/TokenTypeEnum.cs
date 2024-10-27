namespace Server.Infrastruct.Model.Models.Enums
{
    /// <summary>
    /// token类型
    /// </summary>
    public enum TokenTypeEnum
    {
        /// <summary>
        /// 自定义key
        /// </summary>
        ApiKey = 0,

        /// <summary>
        /// jwt生成的key-不存储redis
        /// </summary>
        Jwt = 1,

        /// <summary>
        /// app、secret模式
        /// </summary>
        AppSecret = 2
    }
}
