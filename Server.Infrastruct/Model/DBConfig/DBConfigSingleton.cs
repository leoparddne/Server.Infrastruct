using Common.Toolkit.Helper;
using Newtonsoft.Json;
using Server.Infrastruct.Extension;
using Server.Infrastruct.Model.Models.Constant;

namespace Server.Infrastruct.Model.DBConfig
{
    /// <summary>
    /// 单例
    /// </summary>
    public class DBConfigSingleton : DBConfigBase
    {

        private static DBConfigSingleton Config { get; set; } = null;

        private static readonly object _lock = new object();

        private DBConfigSingleton() { }

        public static DBConfigSingleton GetConfig()
        {
            if (Config != null)
            {
                return Config;
            }

            lock (_lock)
            {
                //双重锁
                if (Config != null)
                {
                    return Config;
                }

                // 预留配置中心逻辑
                string onlineConfigName = AppSettingsHelper.GetSetting(DBConstant.OutterDBConfig);
                if (!string.IsNullOrEmpty(onlineConfigName))
                {
                    string configValue = ConsulEx.GetKV(onlineConfigName);
                    if (string.IsNullOrWhiteSpace(configValue))
                    {
                        throw new Exception("DBConfigError");
                    }
                    return Config = JsonConvert.DeserializeObject<DBConfigSingleton>(configValue);
                }



                //读取配置文件
                Config = new DBConfigSingleton
                {
                    ConnectionString = AppSettingsHelper.GetSetting("ConnectionString"),
                    ConnectionType = AppSettingsHelper.GetSetting("ConnectionType") ?? "Oracle",
                    RedisConnectionString = AppSettingsHelper.GetSetting("RedisConnectionString"),
                    GateWay = AppSettingsHelper.GetSetting("GateWay"),
                    MongoDB = AppSettingsHelper.GetSetting("MongoDB")
                };

                string sqlAutoToLowerStr = AppSettingsHelper.GetSetting("SqlAutoToLower");
                bool sqlAutoToLower = true;

                if (!string.IsNullOrWhiteSpace(sqlAutoToLowerStr))
                {
                    if (bool.TryParse(sqlAutoToLowerStr, out sqlAutoToLower))
                    {
                        Config.SqlAutoToLower = sqlAutoToLower;
                    }
                }


            }

            return Config;
        }
    }
}
