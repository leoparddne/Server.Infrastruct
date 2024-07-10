using Server.Domain.Models.Model;

namespace Server.Infrastruct.WebAPI.ServiceExtension.Authentication
{
    public interface IAuthenticationService
    {
        #region GetUserInfo
        /// <summary>
        /// 获取用户身份信息
        /// </summary>
        /// <returns></returns>
        UserInfoModel GetUserInfo();
        #endregion

        #region GetAuthorization
        /// <summary>
        /// 获取键
        /// </summary>
        /// <returns></returns>
        string GetAuthorization();
        #endregion

        string GetUserNo();

        /// <summary>
        /// 获取用户唯一id
        /// </summary>
        /// <returns></returns>
        string GetUserId();

        /// <summary>
        /// 生成token
        /// <code>方法已弃用，请使用JwtHelper.GenerateToken(); </code>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="expires">过期时间，默认1小时(无用，当前使用redis做相关限制)</param>
        /// <returns></returns>
        [Obsolete("方法已弃用", true)]
        string GenerateToken(object model, int expires = 1);
    }
}
