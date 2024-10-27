using MongoDB.Bson;

namespace Server.Infrastruct.Helper.MongoDB
{
    public interface IMongoDBBaseModel
    {
        public ObjectId ID { get; set; }
    }
}
