using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace Server.Infrastruct.WebAPI.ServiceExtension.AutoAPI.Base
{
    public class AutoAPIControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            //判断是否继承了指定的接口
            if (typeof(IAutoService).IsAssignableFrom(typeInfo))
            {
                if (!typeInfo.IsInterface &&
                    !typeInfo.IsAbstract &&
                    !typeInfo.IsGenericType &&
                    typeInfo.IsPublic)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
