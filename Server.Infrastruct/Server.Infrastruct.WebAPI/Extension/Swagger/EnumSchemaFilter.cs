using Common.Toolkit.Extention;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace Server.Infrastruct.WebAPI.Extension.Swagger
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                StringBuilder stringBuilder = new StringBuilder();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(name =>
                    {
                        Enum e = (Enum)Enum.Parse(context.Type, name);
                        var data = $"{name}({e.GetDesc()})={Convert.ToInt64(Enum.Parse(context.Type, name))}";

                        stringBuilder.AppendLine(data);
                    });
                model.Description = stringBuilder.ToString();


                model.Type = context.Type.Name;
                model.Format = context.Type.Name;
            }
        }

    }
}
