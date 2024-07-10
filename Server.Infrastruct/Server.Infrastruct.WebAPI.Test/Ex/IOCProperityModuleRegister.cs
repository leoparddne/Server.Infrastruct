using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace Server.Infrastruct.WebAPI.Test.Ex
{
    public class IOCProperityModuleRegister : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Type controllerBaseType = typeof(ControllerBase);

            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                        .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                        .PropertiesAutowired();
        }
    }
}
