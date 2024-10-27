using Microsoft.OpenApi.Any;
using Server.Infrastruct.WebAPI.Extension.Autofac;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;

namespace Server.Infrastruct.WebAPI.Extension.Swagger
{
    /// <summary>
    /// swagger文档生成过滤器，用于枚举描述的生成
    /// </summary>
    public class SwaggerEnumFilter : IDocumentFilter
    {
        /// <summary>
        /// 实现IDocumentFilter接口的Apply函数
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(Microsoft.OpenApi.Models.OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            Dictionary<string, Type> dict = GetAllEnum();

            foreach (KeyValuePair<string, Microsoft.OpenApi.Models.OpenApiSchema> item in swaggerDoc.Components.Schemas)
            {
                Microsoft.OpenApi.Models.OpenApiSchema property = item.Value;
                string typeName = item.Key;
                Type itemType = null;
                if (property.Enum != null && property.Enum.Count > 0)
                {
                    if (dict.ContainsKey(typeName))
                    {
                        itemType = dict[typeName];
                    }
                    else
                    {
                        //property.Type = null;
                        //itemType = null;
                    }
                    List<OpenApiInteger> list = new List<OpenApiInteger>();
                    foreach (IOpenApiAny val in property.Enum)
                    {
                        list.Add((OpenApiInteger)val);
                    }
                    property.Description = DescribeEnum(itemType, list);
                }
            }
        }
        private static Dictionary<string, Type> GetAllEnum()
        {
            Dictionary<string, Type> dict = new Dictionary<string, Type>();

            foreach (Assembly item in AutofacModuleRegister.GetAssemblyList())
            {

                //Assembly item = Assembly.Load("Model");//枚举所在的命名空间的xml文件名，我的枚举都放在Model层里（类库）
                Type[] types = item.GetTypes();

                foreach (Type type in types)
                {
                    if (type.IsEnum)
                    {
                        dict.Add(type.Name, type);
                    }
                }
            }

            return dict;
        }

        private static string DescribeEnum(Type type, List<OpenApiInteger> enums)
        {
            List<string> enumDescriptions = new List<string>();
            foreach (OpenApiInteger item in enums)
            {
                if (type == null) continue;
                object value = Enum.Parse(type, item.Value.ToString());
                string desc = GetDescription(type, value);

                if (string.IsNullOrEmpty(desc))
                    enumDescriptions.Add($"{item.Value.ToString()}：{Enum.GetName(type, value)}；");
                else
                    enumDescriptions.Add($"{item.Value.ToString()}：{Enum.GetName(type, value)}，{desc}；");
            }
            return $"<br><div>{Environment.NewLine}{string.Join("<br/>" + Environment.NewLine, enumDescriptions)}</div>";
        }

        private static string GetDescription(Type t, object value)
        {
            foreach (MemberInfo mInfo in t.GetMembers())
            {
                if (mInfo.Name == t.GetEnumName(value))
                {
                    foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))
                    {
                        if (attr.GetType() == typeof(DescriptionAttribute))
                        {
                            return ((DescriptionAttribute)attr).Description;
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}