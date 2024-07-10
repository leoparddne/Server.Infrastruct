using Autofac;

namespace Server.Infrastruct.WebAPI.Extension.Autofac
{
    /// <summary>
    /// 根据库名称加载程序集
    /// </summary>
    public class IOCNameModuleRegister : Module
    {
        private static string libraryName = "MES";


        public static void SetLibraryName(string _libraryName)
        {
            libraryName = _libraryName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            AutofacModuleRegister autofacModuleRegister = new AutofacModuleRegister();

            autofacModuleRegister.Create(builder, libraryName);
        }
    }
}
