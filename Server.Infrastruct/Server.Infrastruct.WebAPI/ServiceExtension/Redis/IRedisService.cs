using StackExchange.Redis;

namespace Server.Infrastruct.WebAPI.ServiceExtension.Redis
{
    public interface IRedisService : IDisposable
    {

        /// <summary>
        /// 切换数据库
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        IDatabase SwitchDatabase(int db = -1);


        #region KeyExist
        /// <summary>
        /// 判断键是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool KeyExist(string key);
        #endregion

        #region Get
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        string Get(string key);

        /// <summary>
        /// 获取数据对象
        /// </summary>
        /// <typeparam name="TEntity">对象</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        TEntity Get<TEntity>(string key);
        #endregion

        #region Set
        /// <summary>
        /// 新增/更新值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns></returns>
        bool Set(string key, object value, TimeSpan? expireTime);
        #endregion

        #region Remove
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool Remove(string key);
        #endregion


        public void ZSetAdd(string key, string value, double score);

        public void ZSetRemove(string key, string value);


        public void ZSetIncrement(string key, string value, double score);

        public void ZSetDecrement(string key, string value, double score);



        public RedisValue[] ZSetGet(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Order order = Order.Ascending);



        public IEnumerable<SortedSetEntry> ZSetScan(string key);

    }
}
