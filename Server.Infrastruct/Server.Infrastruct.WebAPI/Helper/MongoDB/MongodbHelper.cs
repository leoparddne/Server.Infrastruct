using MongoDB.Driver;

namespace Server.Infrastruct.WebAPI.Helper.MongoDB
{
    public class MongodbHelper : IDisposable
    {
        public MongoClient mongoClient = null;

        public MongoConfig Config { get; set; }

        protected IMongoDatabase database = null;

        #region 构造函数
        public MongodbHelper(string connectionString)
        {
            mongoClient = new MongoClient(connectionString);
            Config = new MongoConfig(connectionString);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="db">授权数据库 - 一般为admin</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="timeSpan"></param>
        /// <param name="maxConnectionPoolSize"></param>
        public MongodbHelper(string host, int port, string db, string username, string password, TimeSpan? timeSpan = null, int maxConnectionPoolSize = 2000)
        {
            //mongoClient = new MongoClient(connectionString);

            var config = new MongoClientSettings
            {
                Server = new MongoServerAddress(host, port),
                MaxConnectionPoolSize = maxConnectionPoolSize
            };

            if (timeSpan != null)
            {
                config.ConnectTimeout = timeSpan.Value;
            }

            config.Credential = MongoCredential.CreateCredential(db, username, password);

            mongoClient = new MongoClient(config);
        }

        /// <summary>
        /// 复杂参数构造
        /// </summary>
        /// <param name="config"></param>
        public MongodbHelper(MongoConfig config)
        {
            Config = config;

            mongoClient = new MongoClient(config.mongoSettings);
        }
        #endregion

        /// <summary>
        /// 手动获取数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public IMongoDatabase GetDB(string dbName)
        {
            IMongoDatabase db = mongoClient.GetDatabase(dbName);
            return db;
        }

        /// <summary>
        /// 切换当前使用的数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public IMongoDatabase SwitchDB(string dbName)
        {
            database = GetDB(dbName);
            return database;
        }

        public void Dispose()
        {

        }
    }
}
