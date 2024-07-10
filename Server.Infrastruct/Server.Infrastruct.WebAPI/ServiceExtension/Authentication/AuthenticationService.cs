using Common.Toolkit.Helper;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Server.Domain.Models.ExceptionExtention;
using Server.Domain.Models.Model;
using Server.Infrastruct.WebAPI.BuilderExtension;
using Server.Infrastruct.WebAPI.ServiceExtension.Redis;
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


        /// <summary>
        /// 生成token
        /// <code>方法已弃用，请使用JwtHelper.GenerateToken(); </code>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="expires">过期时间，默认1小时(无用，当前使用redis做相关限制)</param>
        /// <returns></returns>
        [Obsolete("方法已弃用", true)]
        public string GenerateToken(object model, int expires = 1)
        {
            //创建claim
            Claim[] authClaims = new[] {
                new Claim(ClaimTypes.Name,"platform" ),
                new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(model) ),
                new Claim(JwtRegisteredClaimNames.Sub,"platform"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            IdentityModelEventSource.ShowPII = true;
            //签名秘钥 可以放到json文件中

            string key = AppSettingsHelper.GetSetting("TokenSecureKey");

            ExceptionHelper.CheckException(string.IsNullOrWhiteSpace(key), "无法生成token,请检查配置文件ToKenSecureKey");

            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "YLHPlatform",
                    audience: "YLHPlatform",
                   expires: DateTime.Now.AddHours(expires),
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                   );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
