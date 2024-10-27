using Server.Infrastruct.Model.Entity.Base;
using Server.Infrastruct.Model.Models.Constant;
using Server.Infrastruct.Model.Models.Enums;
using SqlSugar;

namespace Server.Infrastruct.Model.Entity
{
    /// <summary>
    /// 通用实体
    /// </summary>
    public class EnableEntity : CommonEntity, IEnabled
    {
        /// <summary>
        /// 启用、禁用
        /// </summary>
        [SugarColumn(ColumnDescription = "启用禁用", ColumnName = "is_enable")]
        public StatesEnum IsEnabled { get; set; } = StatesEnum.Y;
    }
}
