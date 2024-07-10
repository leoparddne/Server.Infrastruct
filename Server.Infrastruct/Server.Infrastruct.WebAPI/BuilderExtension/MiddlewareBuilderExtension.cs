using Autofac.Extensions.DependencyInjection;
using Server.Infrastruct.WebAPI.Middleware;

namespace Server.Infrastruct.WebAPI.BuilderExtension
{
    public static class MiddlewareBuilderExtension
    {
        /// <summary>
        /// 异常中间件注册
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionCatch(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionCatchMiddleware>();
        }

        /// <summary>
        /// API请求日志中间件注册
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAccessLog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UserAccessLogCatchMiddleware>();
        }

        public static void UseAllServices(this IApplicationBuilder app, IServiceCollection services)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var autofacContaniers = app.ApplicationServices.GetAutofacRoot()?.ComponentRegistry?.Registrations;

            app.Map("/allservices", builder => builder.Run(async context =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync("<style>.table2_1 table {width:100%;margin:15px 0}.table2_1 th {background-color:#93DAFF;color:#000000}.table2_1,.table2_1 th,.table2_1 td{font-size:0.95em;text-align:center;padding:4px;border:1px solid #c1e9fe;border-collapse:collapse}.table2_1 tr:nth-child(odd){background-color:#dbf2fe;}.table2_1 tr:nth-child(even){background-color:#fdfdfd;}</style>");

                await context.Response.WriteAsync($"<h3>所有服务{services.Count}个</h3><table class='table2_1'><thead><tr><th>类型</th><th>生命周期</th><th>Instance</th></tr></thead><tbody>");

                foreach (var svc in services)
                {
                    await context.Response.WriteAsync("<tr>");
                    await context.Response.WriteAsync($"<td>{svc.ServiceType.FullName}</td>");
                    await context.Response.WriteAsync($"<td>{svc.Lifetime}</td>");
                    await context.Response.WriteAsync($"<td>{svc.ImplementationType?.Name}</td>");
                    await context.Response.WriteAsync("</tr>");
                }
                foreach (var item in autofacContaniers.ToList())
                {
                    var interfaceType = item.Services;
                    foreach (var typeArray in interfaceType)
                    {
                        await context.Response.WriteAsync("<tr>");
                        await context.Response.WriteAsync($"<td>{typeArray?.Description}</td>");
                        await context.Response.WriteAsync($"<td>{item.Lifetime}</td>");
                        await context.Response.WriteAsync($"<td>{item?.Target.Activator.ToString().Replace("(ReflectionActivator)", "")}</td>");
                        await context.Response.WriteAsync("</tr>");
                    }
                }
                await context.Response.WriteAsync("</tbody></table>");
            }));
        }
    }
}
