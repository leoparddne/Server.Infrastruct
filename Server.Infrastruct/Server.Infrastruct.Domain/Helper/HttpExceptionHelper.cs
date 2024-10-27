using Common.Toolkit.Helper;
using Server.Infrastruct.Model.Models.ExceptionExtention;

namespace Server.Infrastruct.Model.Helper
{
    public static class HttpExceptionHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="errMsg"></param>
        /// <param name="httpCode"></param>
        public static void CheckNull(object obj, string errMsg, int httpCode)
        {
            Check(obj == null, errMsg, httpCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="enumValue"></param>
        /// <param name="httpCode"></param>
        public static void CheckNull(object obj, Enum enumValue, int httpCode)
        {
            Check(obj == null, enumValue, httpCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="exceptio"></param>
        /// <param name="httpCode"></param>
        public static void CheckNull(object obj, Exception exceptio, int httpCode)
        {
            Check(obj == null, exceptio, httpCode);
        }

        /// <summary>
        /// check为true时候自动触发exception
        /// </summary>
        /// <param name="check"></param>
        /// <param name="errMsg"></param>
        /// <param name="httpCode"></param>
        public static void Check(bool check, string errMsg, int httpCode)
        {
            if (!check)
            {
                return;
            }
            Exec(errMsg, httpCode);
        }

        /// <summary>
        /// 如果check为true自动触发exception并且获取枚举上方description作为exception的错误信息
        /// </summary>
        /// <param name="check"></param>
        /// <param name="enumValue"></param>
        /// <param name="httpCode"></param>
        public static void Check(bool check, Enum enumValue, int httpCode)
        {
            Check(check, enumValue.GetDesc(), httpCode);
        }

        /// <summary>
        ///  check为true时候自动触发exception
        /// </summary>
        /// <param name="check"></param>
        /// <param name="exception"></param>
        /// <param name="httpCode"></param>
        public static void Check(bool check, Exception exception, int httpCode)
        {
            if (!check)
            {
                return;
            }
            Exec(exception.Message, httpCode);
        }


        public static void Exec(string message, int httpCode)
        {
            throw new HttpCodeException(httpCode, message);
        }
    }
}
