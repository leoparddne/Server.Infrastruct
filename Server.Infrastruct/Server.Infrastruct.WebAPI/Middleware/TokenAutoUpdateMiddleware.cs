using Microsoft.IdentityModel.Tokens;
using Server.Infrastruct.Model.Models.Constant;
using Server.Infrastruct.Model.Models.Model;
using Server.Infrastruct.Services.Redis;

namespace Server.Infrastruct.WebAPI.Middleware
{
    /// <summary>
    /// 自动更新token
    /// 只处理token的更新逻辑 - 将微服务中的token更新调整到网关
    /// 独立服务需要手动添加此中间件
    /// </summary>
    public class TokenAutoUpdateMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IRedisService redisService;

        public TokenAutoUpdateMiddleware(RequestDelegate next, IRedisService RedisService)
        {
            _next = next;
            redisService = RedisService;
        }

        public async Task Invoke(HttpContext context)
        {
            //更新
            UpdateToken(context);
            await _next(context);
        }

        public void UpdateToken(HttpContext context)
        {
            if (context.Request.Headers.IsNullOrEmpty())
            {
                return;
            }
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                return;
            }
            var authorization = context.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(authorization))
            {
                //throw new UnAuthorizedException();
                return;
            }

            bool keyIsExist = redisService.KeyExist(authorization);
            if (!keyIsExist)
            {
                //判断token
                //throw new TokenExpiredException();
                return;
            }
            UserInfoModel userInfoModel = redisService.Get<UserInfoModel>(authorization);
            //if (DateTime.Now > userInfoModel.ExpireTime)
            //{
            //    //过期移除
            //    redisService.Remove(authorization);

            //    //throw new TokenExpiredException();
            //    return;
            //}
            userInfoModel.ExpireTime = DateTime.Now.AddSeconds(AppConstant.GetRedisExpirationTime());

            redisService.Set(authorization, userInfoModel, new TimeSpan(0, 0, AppConstant.GetRedisExpirationTime()));
        }
    }
}
