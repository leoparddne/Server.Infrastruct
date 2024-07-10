using SqlSugar;

namespace Server.Domain.Entity.Base
{
    /// <summary>
    /// 数据库通用删除字段
    /// </summary>
    public interface IDelete
    {
        /// <summary>
        /// 是否删除 0正常 1删除
        /// </summary>
        [SugarColumn(ColumnDescription = "是否删除 0正常 1删除", ColumnName = "is_delete")]
        public int IsDelete { get; set; }
    }
}
