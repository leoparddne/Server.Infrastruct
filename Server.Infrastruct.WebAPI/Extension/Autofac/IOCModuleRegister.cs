using Autofac;

namespace Server.Infrastruct.WebAPI.Extension.Autofac
{
    public class IOCModuleRegister : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            AutofacModuleRegister autofacModuleRegister = new AutofacModuleRegister();

            autofacModuleRegister.Create(builder);
        }
    }
}
