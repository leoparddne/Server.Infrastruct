using Server.Infrastruct.WebAPI.Helper.MongoDB;

namespace Server.Infrastruct.WebAPI.ServiceExtension.Mongodb
{
    public interface IMongoBaseService<T> : IMongodbBaseHelper<T> where T : IMongoDBBaseModel, new()
    {

    }
}
