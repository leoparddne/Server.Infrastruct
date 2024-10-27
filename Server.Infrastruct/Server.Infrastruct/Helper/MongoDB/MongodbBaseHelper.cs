using MongoDB.Bson;
using MongoDB.Driver;
using Server.Infrastruct.Helper.MongoDB.Attributes;
using System.Linq.Expressions;

namespace Server.Infrastruct.Helper.MongoDB
{
    public class MongodbBaseHelper<T> : MongodbHelper, IMongodbBaseHelper<T> where T : IMongoDBBaseModel, new()
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string DocumentName { get; set; }

        public string DBName { get; set; }

        public IMongoCollection<T> Collection { get; set; }


        #region 构造函数
        public MongodbBaseHelper(string connectionString) : base(connectionString)
        {
            Init();
        }

        public MongodbBaseHelper(MongoConfig config) : base(config)
        {
            Init();
        }

        public MongodbBaseHelper(string host, int port, string db, string username, string password, TimeSpan? timeSpan = null, int maxConnectionPoolSize = 2000) : base(host, port, db, username, password, timeSpan, maxConnectionPoolSize)
        {
            Init();
        }
        #endregion

        #region 扩展

        public void OpenDB(string dbName)
        {
            DBName = dbName;
            SwitchDB(dbName);
            Collection = database.GetCollection<T>(DocumentName);
        }

        /// <summary>
        /// 解析并初始化文档名称 - 外部不应该直接调用此方法
        /// </summary>
        /// <returns></returns>
        private string Init()
        {
            var name = DocumnetNameAttrbute.GetDocumentName<T>();
            return DocumentName = name;
        }
        public UpdateDefinition<T> BuildUpdate<TField>(Expression<Func<T, TField>> field, TField value)
        {
            UpdateDefinition<T> update = Builders<T>.Update.Set(field, value);
            return update;
        }


        #endregion

        #region CRUD

        public void Insert(T data)
        {
            Collection.InsertOne(data);
        }

        public void InsertMany(List<T> data)
        {
            Collection.InsertMany(data);
        }

        public void DeleteByID(string id)
        {
            Collection.DeleteMany(f => f.ID == new ObjectId(id));
        }

        public void DeleteSingle(Expression<Func<T, bool>> expression)
        {
            Collection.DeleteOne(expression);
        }

        public void DeleteMany(Expression<Func<T, bool>> expression)
        {
            Collection.DeleteMany(expression);
        }

        public void Update(Expression<Func<T, bool>> expression, UpdateDefinition<T> update)
        {
            Collection.UpdateOne(expression, update);
        }

        public void UpdateMany(Expression<Func<T, bool>> expression, UpdateDefinition<T> update)
        {
            Collection.UpdateMany(expression, update);
        }

        public void ReplaceOne(Expression<Func<T, bool>> expression, T model)
        {
            Collection.ReplaceOne(expression, model);
        }


        public T FindOne(Expression<Func<T, bool>> expression)
        {
            return Collection.AsQueryable().FirstOrDefault(expression);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return Collection.AsQueryable().Where(expression);
        }

        #endregion
    }
}
