using SqlSugar;

namespace Server.Domain.Entity.Base
{
    /// <summary>
    /// 数据库通用ID接口
    /// </summary>
    public interface ICommonID
    {
        /// <summary>
        /// 通用ID字段
        /// </summary>
        [SugarColumn(ColumnDescription = "创建人", ColumnName = "ID")]
        public string Id { get; set; }
    }
}
