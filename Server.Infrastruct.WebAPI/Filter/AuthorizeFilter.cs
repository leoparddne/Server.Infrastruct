using Common.Toolkit.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Infrastruct.Extension;
using Server.Infrastruct.Model.Models.Constant;
using Server.Infrastruct.Model.Models.ExceptionExtention;
using Server.Infrastruct.Model.Models.Model;
using Server.Infrastruct.Services.Redis;

namespace Server.Infrastruct.WebAPI.Filter
{
    public class AuthorizeFilter : IActionFilter
    {
        private IRedisService? redisService;
        private readonly IServiceProvider? serviceProvider;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public AuthorizeFilter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {

        }


        /// <summary>
        /// 授权过滤
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // 包含AllowAnonymousAttribute跳过后续逻辑
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                bool isDefined = controllerActionDescriptor.EndpointMetadata.Any(a => a.GetType().Equals(typeof(AllowAnonymousAttribute)));
                if (isDefined)
                {
                    return;
                }
            }

            string skipLogin = AppSettingsHelper.GetSetting("FilterSetting", "SkipLogin");
            if (!string.IsNullOrEmpty(skipLogin) && skipLogin.Contains(context.HttpContext.Request.Path.Value))
            {
                return;
            }

            APIResponseModel<string> result = new APIResponseModel<string>();
            string authorization = context.HttpContext.Request.Headers["Authorization"];

            if (DebugExtension.ISDebug)
            {
                string debugToken = AppSettingsHelper.GetSetting("DebugToken");
                if (!string.IsNullOrWhiteSpace(debugToken))
                {
                    authorization = debugToken;
                    return;
                }
            }



            if (string.IsNullOrWhiteSpace(authorization))
            {
                throw new UnAuthorizedException();
            }


            redisService = serviceProvider.GetService<IRedisService>();


            bool keyIsExist = redisService.KeyExist(authorization);
            if (!keyIsExist)
            {
                //判断token
                throw new TokenExpiredException();
            }
            UserInfoModel userInfoModel = redisService.Get<UserInfoModel>(authorization);
            if (DateTime.Now > userInfoModel.ExpireTime)
            {
                //过期移除
                redisService.Remove(authorization);

                throw new TokenExpiredException();
            }

            //TODO - 需要统一调整token逻辑时移除此处更新逻辑
            userInfoModel.ExpireTime = DateTime.Now.AddSeconds(AppConstant.GetRedisExpirationTime());

            redisService.Set(authorization, userInfoModel, new TimeSpan(0, 0, AppConstant.GetRedisExpirationTime()));
        }
    }
}
