using Common.Toolkit.Helper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json.Serialization;
using Server.Domain.Models.Enums;
using Server.Domain.Models.Model;
using Server.Infrastruct.WebAPI.Filter;
using Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI.Base;

namespace Server.Infrastruct.WebAPI.CollectionExtension
{
    public static class BasicCollectionExtension
    {
        /// <summary>
        /// 基础注册
        /// </summary>
        /// <param name="services"></param>
        public static void AddBasic(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddCors(t => t.AddPolicy("any", p =>
            {
                p.AllowAnyOrigin();
                p.AllowAnyMethod();
                p.AllowAnyHeader();
            }));
            services.AddControllers()
                .AddNewtonsoftJson(n =>
                {
                    n.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    n.SerializerSettings.ContractResolver = new DefaultContractResolver();
                }).AddDynamicWebApi();


            services.Configure<KestrelServerOptions>(t =>
            {
                t.AllowSynchronousIO = true;
                t.Limits.MaxRequestBodySize = long.Parse(AppSettingsHelper.GetSetting("FileSize", "Kestrel"));
            });

            services.Configure<IISServerOptions>(t =>
            {
                t.AllowSynchronousIO = true;
                t.MaxRequestBodySize = long.Parse(AppSettingsHelper.GetSetting("FileSize", "IIS"));
            });

            services.Configure<FormOptions>(t =>
            {
                t.ValueLengthLimit = int.MaxValue;
                t.MultipartBodyLengthLimit = int.MaxValue;
                t.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddMvc().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            //参数验证
            services.Configure<ApiBehaviorOptions>(options =>
            {
                //只有在数据验证失败时才会执行
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    string errorMessage = string.Empty;
                    if (!context.ModelState.IsValid)
                    {
                        if (!context.ModelState.Values.IsNullOrEmpty())
                        {
                            var modelList = context.ModelState.Values.ToList();
                            var invalidList = modelList.Where(f => f.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)?.ToList();
                            if (!invalidList.IsNullOrEmpty())
                            {
                                var firstModel = invalidList.FirstOrDefault();
                                if (!firstModel.Errors.IsNullOrEmpty())
                                {
                                    errorMessage = firstModel.Errors.FirstOrDefault().ErrorMessage;
                                }
                            }
                        }
                    }


                    return new JsonResult(new APIResponseModel<object> { Code = ResponseEnum.Fail.GetHashCode(), Message = errorMessage, Data = null });
                };
            });

            services.AddSingleton<AuthorizeFilter>();
        }
    }
}
