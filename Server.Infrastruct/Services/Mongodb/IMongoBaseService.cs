using Server.Infrastruct.Helper.MongoDB;

namespace Server.Infrastruct.Services.Mongodb
{
    public interface IMongoBaseService<T> : IMongodbBaseHelper<T> where T : IMongoDBBaseModel, new()
    {

    }
}
