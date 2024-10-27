using System.ComponentModel;

namespace Server.Infrastruct.Model.Models.Enums
{
    /// <summary>
    /// 返回值代码
    /// </summary>
    public enum ResponseEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 0,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = 1,

        /// <summary>
        /// 未授权
        /// </summary>
        [Description("未授权")]
        Unauthorized = 401,

        /// <summary>
        /// 授权失效
        /// </summary>
        [Description("授权失效")]
        TimeExpired = 430,

        /// <summary>
        /// 多语言信息返回
        /// </summary>
        [Description("多语言信息返回")]
        LangageInfo = 450,

        /// <summary>
        /// 导入异常返回
        /// </summary>
        [Description("导入异常返回")]
        ImportInfo = 452,

    }
}
