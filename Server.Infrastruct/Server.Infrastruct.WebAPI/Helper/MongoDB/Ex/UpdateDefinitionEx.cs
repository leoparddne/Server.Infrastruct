using MongoDB.Driver;

namespace Server.Infrastruct.WebAPI.Helper.MongoDB.Ex
{
    public static class UpdateDefinitionEx<T> where T : IMongoDBBaseModel, new()
    {
        public static UpdateDefinitionBuilder<T> GetUpdateBuild()
        {
            UpdateDefinitionBuilder<T> update = Builders<T>.Update;
            return update;
        }

        public static void Set(UpdateDefinitionBuilder<T> update, Dictionary<string, object> value)
        {
            foreach (var item in value)
            {
                update.Set(item.Key, item.Value);
            }
        }
    }
}
