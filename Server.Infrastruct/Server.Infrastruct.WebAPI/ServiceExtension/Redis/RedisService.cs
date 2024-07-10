using Newtonsoft.Json;
using Server.Infrastruct.WebAPI.Model;
using StackExchange.Redis;
using System.Text;

namespace Server.Infrastruct.WebAPI.ServiceExtension.Redis
{
    public class RedisService : IRedisService, IDisposable
    {
        private static DBConfigSingleton config;

        public ConnectionMultiplexer redisConnection;

        private readonly object redisConnectionLock = new object();
        public IDatabase redisDatabase;

        static RedisService()
        {
            config = DBConfigSingleton.GetConfig();
        }

        public RedisService()
        {

            redisConnection = GetRedisConnection();
            SwitchDatabase();
        }

        private ConnectionMultiplexer GetRedisConnection()
        {
            if (redisConnection != null && redisConnection.IsConnected)
            {
                return redisConnection;
            }

            lock (redisConnectionLock)
            {
                if (redisConnection != null)
                {
                    redisConnection.Dispose();
                }

                //TODO
                redisConnection = ConnectionMultiplexer.Connect(config.RedisConnectionString);
                //redisConnection = ConnectionMultiplexer.Connect(new ConfigurationOptions()
                //{
                //    AbortOnConnectFail = false,
                //    AllowAdmin = true,
                //    ConnectTimeout = 15000,
                //    SyncTimeout = 5000,
                //    EndPoints = { redisConnectionString },
                //    Password = "YTBk6XKhY3qm27QZ"
                //});

                return redisConnection;
            }
        }

        /// <summary>
        /// 切换数据库
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IDatabase SwitchDatabase(int db = -1)
        {
            redisDatabase = redisConnection.GetDatabase(db);
            return redisDatabase;
        }

        #region KeyExist
        /// <summary>
        /// 判断键是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool KeyExist(string key)
        {
            return redisDatabase.KeyExists(key);
        }
        #endregion

        #region Get
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string Get(string key)
        {
            return redisDatabase.StringGet(key);
        }

        /// <summary>
        /// 获取数据对象
        /// </summary>
        /// <typeparam name="TEntity">对象</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public TEntity Get<TEntity>(string key)
        {
            RedisValue redisValue = redisDatabase.StringGet(key);
            if (redisValue.HasValue)
            {
                return JsonConvert.DeserializeObject<TEntity>(redisValue);
            }
            else
            {
                return default;
            }
        }
        #endregion

        #region Set
        /// <summary>
        /// 新增/更新值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns></returns>
        public bool Set(string key, object value, TimeSpan? expireTime)
        {

            if (value != null)
            {
                if (value is string cacheValue)
                {
                    return redisDatabase.StringSet(key, cacheValue, expireTime);
                }
                else
                {
                    string valueString = JsonConvert.SerializeObject(value);
                    byte[] valueBytes = Encoding.UTF8.GetBytes(valueString);
                    return redisDatabase.StringSet(key, valueBytes, expireTime);
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Remove
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return redisDatabase.KeyDelete(key);
        }

        public void Dispose()
        {
            redisConnection.Dispose();
        }
        #endregion



        #region zset
        public void ZSetAdd(string key, string value, double score)
        {
            redisDatabase.SortedSetAdd(key, value, score);
        }

        public void ZSetRemove(string key, string value)
        {
            redisDatabase.SortedSetRemove(key, value);
        }


        public void ZSetIncrement(string key, string value, double score)
        {
            redisDatabase.SortedSetIncrement(key, value, score);
        }

        public void ZSetDecrement(string key, string value, double score)
        {
            redisDatabase.SortedSetDecrement(key, value, score);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public RedisValue[] ZSetGet(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Order order = Order.Ascending)
        {
            var value = redisDatabase.SortedSetRangeByScore(key, start, stop, Exclude.None, order);

            return value;
        }

        public IEnumerable<SortedSetEntry> ZSetScan(string key)
        {
            return redisDatabase.SortedSetScan(key);
        }
        #endregion
    }
}
