using Common.Toolkit.Helper;

namespace Server.Infrastruct.WebAPI.Test
{
    public class Program
    {

        private static string ip;
        private static string port;

        public static void Main(string[] args)
        {

            ip = AppSettingsHelper.GetSetting("Publish", "ApplicationIP");
            port = AppSettingsHelper.GetSetting("Publish", "ApplicationPort");

            var builder = WebApplication.CreateBuilder(args);

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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}