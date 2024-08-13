using Autofac.Extensions.DependencyInjection;
using Autofac;
using Common.Toolkit.Helper;
using Server.Infrastruct.WebAPI.BuilderExtension;
using Server.Infrastruct.WebAPI.CollectionExtension;
using Server.Infrastruct.WebAPI.Extension.Autofac;
using Server.Infrastruct.WebAPI.Test.Ex.AutoMapper;
using Server.Infrastruct.WebAPI.Test.Ex;

namespace Server.Infrastruct.WebAPI.Test
{
    public class Program
    {

        private static string ip;
        private static string port;
        private static IServiceCollection Services;

        public static void Main(string[] args)
        {

            ip = AppSettingsHelper.GetSetting("Publish", "ApplicationIP");
            port = AppSettingsHelper.GetSetting("Publish", "ApplicationPort");

            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseUrls($"http://{ip}:{port}");//单个设置


            if (System.OperatingSystem.IsWindows())
            {
                builder.Host.UseWindowsService();
            }

            if (System.OperatingSystem.IsLinux())
            {
                builder.Host.UseSystemd();
            }

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCustomSwaggerHeader();
            builder.Services.AddSqlSugar(log => { global::System.Console.WriteLine(log); });
            builder.Services.AddAutoMapper();

            Services = builder.Services;


            builder.Host
              .UseServiceProviderFactory(new AutofacServiceProviderFactory())
              .ConfigureContainer<ContainerBuilder>(build =>
              {

                  //IOCNameModuleRegister.SetLibraryName("Server.EddySuite");
                  //build.RegisterModule(new IOCNameModuleRegister());

                  build.RegisterModule(new IOCModuleRegister());
                  build.RegisterModule(new IOCProperityModuleRegister());
              });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseConsulRegist();
            }

            app.UseCustomSwagger();
            app.UseExceptionCatch();
            app.UseAuthentication();
            app.UseAccessLog();
            app.UseRouting();
            app.UseCors("any");


            app.UseAuthorization();

            app.UseAllServices(Services);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MapControllers();

            app.Run();
        }
    }
}