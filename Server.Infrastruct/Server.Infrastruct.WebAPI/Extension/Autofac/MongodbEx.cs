using Autofac;
using Server.Infrastruct.WebAPI.ServiceExtension.Mongodb;

namespace Server.Infrastruct.WebAPI.Extension.Autofac
{
    public static class MongodbEx
    {
        public static ContainerBuilder MongodbDI(this ContainerBuilder builder)
        {

            builder.RegisterGeneric(typeof(MongoBaseService<>)).As(typeof(IMongoBaseService<>)).InstancePerLifetimeScope();

            return builder;
        }

    }
}
