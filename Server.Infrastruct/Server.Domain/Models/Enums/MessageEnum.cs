using System.ComponentModel;

namespace Server.Domain.Models.Enums
{
    /// <summary>
    /// 通用返回消息
    /// </summary>
    public enum MessageEnum
    {
        /// <summary>
        /// 字段不存在
        /// </summary>
        [Description("FieldNotFound")]
        FieldNotFound = 1,

        /// <summary>
        /// 数据库类型不支持
        /// </summary>
        [Description("DBTypeNotSupport")]
        DBTypeNotSupport = 2,

        /// <summary>
        /// 无数据
        /// </summary>
        [Description("NoData")]
        NoData = 3,
    }
}
