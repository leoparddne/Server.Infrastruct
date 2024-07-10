using Microsoft.AspNetCore.Mvc;
using Server.Domain.Models.Enums;
using Server.Domain.Models.Model;
using Server.Infrastruct.WebAPI.Filter;

namespace Server.Infrastruct.WebAPI.Controllers
{
    [APIResultFilter]
    [ApiController]
    [ServiceFilter(typeof(AuthorizeFilter))]
    public class APIBaseController : ControllerBase
    {
        #region Sucess
        /// <summary>
        /// 返回成功对象
        /// </summary>
        /// <typeparam name="T">类型对象</typeparam>
        /// <param name="data">数据对象</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        [NonAction]
        public APIResponseModel<T> Success<T>(T data, string message = "")
        {
            return new APIResponseModel<T>()
            {
                Code = ResponseEnum.Success.GetHashCode(),
                Data = data,
                Message = message
            };
        }
        #endregion

        #region Fail
        /// <summary>
        /// 返回失败对象
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        [NonAction]
        public APIResponseModel<string> Fail(string message)
        {
            return new APIResponseModel<string>()
            {
                Code = ResponseEnum.Fail.GetHashCode(),
                Message = message
            };
        }

        /// <summary>
        /// 返回失败对象
        /// </summary>
        /// <typeparam name="T">类型对象</typeparam>
        /// <param name="data">数据对象</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        [NonAction]
        public APIResponseModel<T> Fail<T>(T data, string message)
        {
            return new APIResponseModel<T>()
            {
                Code = ResponseEnum.Fail.GetHashCode(),
                Data = data,
                Message = message
            };
        }
        #endregion
    }
}
