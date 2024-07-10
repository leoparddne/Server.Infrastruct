using Common.Toolkit.Helper;

namespace Server.Infrastruct.WebAPI.Model.Constant
{
    public class AppConstant
    {
        #region 是否启用
        /// <summary>
        /// 未启用
        /// </summary>
        public const string NOTENABLED = "N";

        /// <summary>
        /// 已启用
        /// </summary>
        public const string ENABLED = "Y";
        #endregion


        public const string Y = "Y";


        public const string N = "N";

        #region 存储过程返回成功
        public const string PROCEDURE_SUCCESS = "OK";
        #endregion

        #region redis 配置
        /// <summary>
        /// 过期时间 秒（当前表示6小时）
        /// </summary>
        private static int RedisExpirationSeconds = 6 * 60 * 60;

        /// <summary>
        /// 获取redis过期秒
        /// </summary>
        /// <returns></returns>
        public static int GetRedisExpirationTime()
        {
            string ConfigSeconds = AppSettingsHelper.GetSetting("RedisExpirationSeconds");
            if (!string.IsNullOrEmpty(ConfigSeconds))
            {
                _ = int.TryParse(ConfigSeconds, out RedisExpirationSeconds);
            }
            return RedisExpirationSeconds;
        }

        #endregion
    }
}
