using MongoDB.Driver;
using System.Linq.Expressions;

namespace Server.Infrastruct.WebAPI.Helper.MongoDB
{
    public interface IMongodbBaseHelper<T> where T : IMongoDBBaseModel, new()
    {
        IMongoCollection<T> Collection { get; set; }
        string DBName { get; set; }
        string DocumentName { get; set; }

        UpdateDefinition<T> BuildUpdate<TField>(Expression<Func<T, TField>> field, TField value);
        void DeleteByID(string id);
        void DeleteMany(Expression<Func<T, bool>> expression);
        void DeleteSingle(Expression<Func<T, bool>> expression);
        IQueryable<T> Find(Expression<Func<T, bool>> expression);
        T FindOne(Expression<Func<T, bool>> expression);
        void Insert(T data);
        void InsertMany(List<T> data);
        void OpenDB(string dbName);
        void ReplaceOne(Expression<Func<T, bool>> expression, T model);
        void Update(Expression<Func<T, bool>> expression, UpdateDefinition<T> update);
        void UpdateMany(Expression<Func<T, bool>> expression, UpdateDefinition<T> update);
    }
}