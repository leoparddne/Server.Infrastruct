using Common.Toolkit.Helper;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Server.Infrastruct.Extension;
using Server.Infrastruct.Model.Models.ExceptionExtention;
using Server.Infrastruct.Model.Models.Model;
using Server.Infrastruct.Services.Authentication;
using Server.Infrastruct.Services.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Infrastruct.WebAPI.ServiceExtension.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private IRedisService redisService;

        private IHttpContextAccessor httpContextAccessor;


        public AuthenticationService(IRedisService RedisService, IHttpContextAccessor HttpContextAccessor)
        {
            redisService = RedisService;
            httpContextAccessor = HttpContextAccessor;
        }


        #region GetUserInfo
        /// <summary>
        /// 获取用户身份信息
        /// </summary>
        /// <returns></returns>
        public UserInfoModel GetUserInfo()
        {
            string authorization = GetToken();
            if (!string.IsNullOrWhiteSpace(authorization))
            {
                bool isKeyExist = redisService.KeyExist(authorization);
                if (isKeyExist)
                {
                    return redisService.Get<UserInfoModel>(authorization);
                }
            }

            throw new TokenExpiredException();
        }
        #endregion



        #region GetAuthorization
        /// <summary>
        /// 获取键
        /// </summary>
        /// <returns></returns>
        public string GetAuthorization()
        {
            string authorization = GetToken();

            return authorization;
        }

        private string GetToken()
        {
            string token = httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (DebugExtension.ISDebug)
            {
                string debugToken = AppSettingsHelper.GetSetting("DebugToken");
                if (!string.IsNullOrWhiteSpace(debugToken))
                {
                    token = debugToken;
                }
            }

            return token;
        }

        public string GetUserNo()
        {
            return GetUserInfo().UserNo;
        }

        public string GetUserId()
        {
            return GetUserInfo().UserId;
        }
        #endregion

    }
}
