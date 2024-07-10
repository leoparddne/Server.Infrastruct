using MongoDB.Bson;

namespace Server.Infrastruct.WebAPI.Helper.MongoDB
{
    public interface IMongoDBBaseModel
    {
        public ObjectId Id { get; set; }
    }
}
