using Server.Infrastruct.Helper.MongoDB;
using Server.Infrastruct.Model.DBConfig;

namespace Server.Infrastruct.Services.Mongodb
{
    public class MongoBaseService<T> : MongodbBaseHelper<T>, IMongoBaseService<T> where T : IMongoDBBaseModel, new()
    {

        public MongoBaseService() : base(DBConfigSingleton.GetConfig().MongoDB)
        {
        }
    }
}
