using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Server.Infrastruct.Filter;
using Server.Infrastruct.Model.Models.Model;

namespace Server.Infrastruct.WebAPI.Extension.Swagger
{
    public class ProduceResponseTypeModelProvider : IApplicationModelProvider
    {
        public int Order => 0;

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (ControllerModel controller in context.Result.Controllers)
            {
                foreach (ActionModel action in controller.Actions)
                {
                    if (!action.Filters.Any(e => e is ProducesResponseTypeAttribute producesResponseType && producesResponseType.StatusCode == StatusCodes.Status200OK))
                    {
                        //跳过带有ApiIgnoreAttribute的action
                        if (action.Attributes.Any(f => f is ApiIgnoreAttribute))
                        {
                            continue;
                        }
                        if (action.ActionMethod.ReturnType != null)
                        {
                            Type type = typeof(APIResponseModel<>);

                            //忽略原先旧接口返回类型,后续可移除
                            if (action.ActionMethod.ReturnType.IsGenericType && action.ActionMethod.ReturnType.GetGenericTypeDefinition() == type)
                            {
                                continue;
                            }

                            Type retType = null;
                            if (action.Attributes.Any(f => f is SwaggerRetDataTypeAttribute))
                            {
                                retType = ((SwaggerRetDataTypeAttribute)action.Attributes.First(f => f is SwaggerRetDataTypeAttribute)).RetType;
                            }

                            try
                            {
                                if (action.ActionMethod.ReturnType != typeof(void))
                                {
                                    type = type.MakeGenericType(retType == null ? action.ActionMethod.ReturnType : retType);
                                }
                            }
                            catch (Exception)
                            {

                            }
                            action.Filters.Add(new ProducesResponseTypeAttribute(type, StatusCodes.Status200OK));
                        }
                    }
                }
            }
        }
    }
}
