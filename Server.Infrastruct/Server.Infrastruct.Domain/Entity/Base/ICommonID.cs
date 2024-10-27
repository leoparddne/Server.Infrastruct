using SqlSugar;

namespace Server.Infrastruct.Model.Entity.Base
{
    /// <summary>
    /// 数据库通用ID接口
    /// </summary>
    public interface ICommonID
    {
        /// <summary>
        /// 通用ID字段
        /// </summary>
        [SugarColumn(ColumnDescription = "", ColumnName = "ID")]
        public string ID { get; set; }
    }
}
