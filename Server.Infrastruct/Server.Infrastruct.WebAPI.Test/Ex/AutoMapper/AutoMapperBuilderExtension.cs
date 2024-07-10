
namespace Server.Infrastruct.WebAPI.Test.Ex.AutoMapper
{
    public static class AutoMapperBuilderExtension
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperConfig));
            AutoMapperConfig.RegisterMapping();
        }
    }
}
