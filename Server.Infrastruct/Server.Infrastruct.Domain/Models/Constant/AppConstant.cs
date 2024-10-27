using Common.Toolkit.Helper;

namespace Server.Infrastruct.Model.Models.Constant
{
    public class AppConstant
    {
        public const string Y = "Y";

        public const string N = "N";

        public const string PROCEDURE_SUCCESS = "OK";

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
