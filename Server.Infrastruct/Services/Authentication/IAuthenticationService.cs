using Server.Infrastruct.Model.Models.Model;

namespace Server.Infrastruct.Services.Authentication
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

    }
}
