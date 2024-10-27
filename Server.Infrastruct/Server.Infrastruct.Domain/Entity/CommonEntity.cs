using Server.Infrastruct.Model.Entity.Base;
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Infrastruct.Model.Entity
{
    /// <summary>
    /// 通用实体
    /// </summary>
    public class CommonEntity : ICommonID
    {
        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnDescription = "创建人", ColumnName = "create_user")]
        [Column("create_user")]
        [MaxLength(40)]
        public string CreateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDescription = "创建时间", ColumnName = "create_time")]
        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [SugarColumn(ColumnDescription = "更新人", ColumnName = "update_user")]
        [Column("update_user")]
        [MaxLength(40)]
        public string UpdateUser { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(ColumnDescription = "更新时间", ColumnName = "update_time")]
        [Column("update_time")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 通用ID字段
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "", ColumnName = "ID")]
        public string ID { get; set; }

        /// <summary>
        /// 更新字段调整为当前时间
        /// </summary>
        /// <param name="userNo"></param>
        public void Create(string userNo)
        {
            CreateTime = DateTime.Now;
            CreateUser = userNo;
            Update(userNo);
        }
        /// <summary>
        /// 创建字段调整为当前时间
        /// </summary>
        /// <param name="userNo"></param>
        public void Update(string userNo)
        {
            UpdateTime = DateTime.Now;
            UpdateUser = userNo;
        }
    }
}
