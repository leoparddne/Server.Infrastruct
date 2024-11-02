﻿using Common.Toolkit.Helper;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Server.Infrastruct.WebAPI.Extension.Swagger;

namespace Server.Infrastruct.WebAPI.CollectionExtension
{
    public static class SwaggerCollectionExtension
    {
        /// <summary>
        /// Swagger注册
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {
            string apiName = AppSettingsHelper.GetSetting("SwaggerSetting", "ApiName");
           
            string swaggerXmlName = AppSettingsHelper.GetSetting("SwaggerSetting", "XmlName") ?? "";

            string basePath = AppContext.BaseDirectory;

            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc(apiName, new OpenApiInfo
                //{
                //    Title = $"{apiName}接口"
                //});

                if (!string.IsNullOrWhiteSpace(swaggerXmlName.Trim()))
                {
                    var file = swaggerXmlName.Split(',');
                    foreach (var item in file)
                    {
                        string xmlPath = Path.Combine(basePath, item);
                        if (File.Exists(xmlPath))
                        {
                            c.IncludeXmlComments(xmlPath, true);
                        }
                    }
                }

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference{ Type=ReferenceType.SecurityScheme,Id="Bearer"}
                        },
                        new List<string>()
                    }
                });

                //枚举注释
                c.SchemaFilter<EnumSchemaFilter>();

                //c.DocumentFilter<SwaggerEnumFilter>();
            });
        }

        /// <summary>
        /// 自动化添加返回值外侧结构(ProducesResponseType)
        /// </summary>
        /// <param name="services"></param>
        public static void AddCustomSwaggerHeader(this IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, ProduceResponseTypeModelProvider>());
        }
    }
}
