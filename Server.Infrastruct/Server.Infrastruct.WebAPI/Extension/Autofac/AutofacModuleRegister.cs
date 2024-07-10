using Autofac;
using Microsoft.Extensions.DependencyModel;
using Server.Infrastruct.WebAPI.Repository;
using Server.Infrastruct.WebAPI.ServiceExtension;
using Server.Infrastruct.WebAPI.ServiceExtension.Agent;
using Server.Infrastruct.WebAPI.ServiceExtension.Authentication;
using Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI;
using Server.Infrastruct.WebAPI.ServiceExtension.Redis;
using Server.Infrastruct.WebAPI.UnitOfWork;
using System.Reflection;
using System.Runtime.Loader;

namespace Server.Infrastruct.WebAPI.Extension.Autofac
{
    public class AutofacModuleRegister
    {
        /// <summary>
        /// Autofac创建
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="libraryName"></param>
        /// <returns></returns>
        public ContainerBuilder Create(ContainerBuilder builder, string libraryName = "MES")
        {
            BaseCreate(builder, libraryName);
            AuthenticationCreate(builder);
            RawHttpAgentServiceCreate(builder);
            HttpAgentServiceCreate(builder);
            return RedisCreate(builder);
        }


        /// <summary>
        /// Autofac创建
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="libraryName"></param>
        /// <returns></returns>
        public ContainerBuilder BaseCreate(ContainerBuilder builder, string libraryName = "MES")
        {
            List<Assembly> assemblyList = GetAssemblyList(libraryName);

            builder.RegisterAssemblyTypes(assemblyList.ToArray())
                    .AsImplementedInterfaces()
                    .PropertiesAutowired()
                    .InstancePerLifetimeScope();

            builder.RegisterType(typeof(BaseService)).As(typeof(IBaseService)).SingleInstance();
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).SingleInstance();
            builder.RegisterGeneric(typeof(DBService<>)).As(typeof(IDBService<>)).SingleInstance();
            builder.RegisterType<BaseRepositoryExtension>().As<IBaseRepositoryExtension>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();

            #region autoservice
            builder.RegisterGeneric(typeof(AutoServiceCommonID<>)).As(typeof(IAutoServiceCommonID<>)).SingleInstance();
            builder.RegisterGeneric(typeof(AutoServiceCommon<>)).As(typeof(IAutoServiceCommon<>)).SingleInstance();
            builder.RegisterGeneric(typeof(AutoServiceBase<>)).As(typeof(IAutoServiceBase<>)).SingleInstance();
            builder.RegisterGeneric(typeof(AuthServiceCommon<>)).As(typeof(IAuthServiceCommon<>)).SingleInstance();
            builder.RegisterGeneric(typeof(DBService<>)).As(typeof(IDBService<>)).SingleInstance();
            #endregion


            builder.RegisterType<UnitOfWork.UnitOfWork>().As<IUnitOfWork>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().AsImplementedInterfaces().PropertiesAutowired().InstancePerLifetimeScope();
            return builder;
        }



        /// <summary>
        /// Autofac创建
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ContainerBuilder AuthenticationCreate(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            return builder;
        }


        /// <summary>
        /// Autofac创建
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ContainerBuilder HttpAgentServiceCreate(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(HttpAgentService)).As(typeof(IHttpAgentService)).InstancePerLifetimeScope();

            return builder;
        }

        /// <summary>
        /// Autofac创建
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ContainerBuilder RawHttpAgentServiceCreate(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(RawHttpAgentService)).As(typeof(IRawHttpAgentService)).InstancePerLifetimeScope();

            return builder;
        }


        /// <summary>
        /// Autofac创建
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ContainerBuilder RedisCreate(ContainerBuilder builder)
        {
            builder.RegisterType<RedisService>().As<IRedisService>().AsImplementedInterfaces().PropertiesAutowired().SingleInstance();
            return builder;
        }

        public static List<Assembly> GetAssemblyList(string libraryName = "MES")
        {
            List<Assembly> assemblyList = new List<Assembly>();
            DependencyContext dependencyContext = DependencyContext.Default;

            dependencyContext.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package")
                .ToList().ForEach(item =>
                {
                    if (item.Name.Contains(libraryName))
                    {
                        Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(item.Name));
                        assemblyList.Add(assembly);
                    }
                });

            return assemblyList;
        }
    }
}
