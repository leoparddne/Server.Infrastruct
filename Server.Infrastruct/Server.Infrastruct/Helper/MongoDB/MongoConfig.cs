using MongoDB.Driver;

namespace Server.Infrastruct.Helper.MongoDB
{
    public class MongoConfig
    {
        /// <summary>
        /// 使用连接字符串模式时候使用此字段
        /// </summary>
        public readonly string ConnectionStr;

        public readonly MongoClientSettings mongoSettings;

        /// <summary>
        /// 超时时间
        /// </summary>
        public TimeSpan TimeoutSpan { get; set; } = new(3 * 1000);

        public readonly MongoCredential Credential;

        public MongoConfig(string connectionStr)
        {
            ConnectionStr = connectionStr;
        }

        /// <summary>
        /// 单服务器指定ip、端口
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="credential"></param>
        /// <param name="timeSpan"></param>
        /// <param name="maxConnectionPoolSize"></param>
        public MongoConfig(string host, int port, MongoCredential credential = null, TimeSpan? timeSpan = null, int maxConnectionPoolSize = 2000) : this(new List<MongoServerAddress> { new MongoServerAddress(host, port) }, credential, timeSpan, maxConnectionPoolSize)
        {

        }

        /// <summary>
        /// 多个服务
        /// </summary>
        /// <param name="server"></param>
        /// <param name="credential"></param>
        /// <param name="timeSpan">设置连接超时时长 - 默认3s</param>
        /// <param name="maxConnectionPoolSize">设置连接池最大连接数 - 默认2000</param>
        public MongoConfig(List<MongoServerAddress> server,
            MongoCredential credential = null, TimeSpan? timeSpan = null, int maxConnectionPoolSize = 2000)
        {
            //超时、授权
            mongoSettings = new MongoClientSettings();

            //重设超时时长
            if (timeSpan != null)
            {
                TimeoutSpan = timeSpan.Value;
            }
            mongoSettings.ConnectTimeout = TimeoutSpan;
            mongoSettings.MaxConnectionPoolSize = maxConnectionPoolSize;

            mongoSettings.Servers = server;//服务器地址
            mongoSettings.ReadPreference = new ReadPreference(ReadPreferenceMode.Primary);

            //MongoClient client = new MongoClient(mongoSettings);

            if (credential != null)
            {
                mongoSettings.Credential = credential;
            }

            Credential = mongoSettings.Credential;
        }
    }
}
