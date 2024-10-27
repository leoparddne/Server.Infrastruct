using Common.Toolkit.Helper;
using Newtonsoft.Json;
using Server.Infrastruct.Extension;
using Server.Infrastruct.Model.Models.Constant;

namespace Server.Infrastruct.Model.DBConfig
{
    /// <summary>
    /// 单例
    /// </summary>
    public class DBConfigMutiSingleton : DBConfigBase
    {
        private static List<DBConfigBase> Config { get; set; } = null;

        private static readonly object _lock = new object();

        private DBConfigMutiSingleton() { }

        public static List<DBConfigBase> GetConfig()
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
                    return Config = JsonConvert.DeserializeObject<List<DBConfigBase>>(configValue);
                }



                //读取配置文件
                Config = AppSettingsHelper.GetObject<List<DBConfigBase>>("DBList");
            }

            return Config;
        }
    }
}
