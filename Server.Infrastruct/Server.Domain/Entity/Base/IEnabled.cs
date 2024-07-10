using SqlSugar;

namespace Server.Domain.Entity.Base
{
    /// <summary>
    /// 数据库通用启用、禁用字段
    /// </summary>
    public interface IEnabled
    {
        /// <summary>
        /// 启用、禁用
        /// </summary>
        [SugarColumn(ColumnDescription = "启用禁用", ColumnName = "is_enabled")]
        public string IsEnabled { get; set; }
    }
}
