using Server.Domain.Models.Enums.Search;

namespace Server.Domain.Dto.InDto.Search
{
    /// <summary>
    /// 搜索条件,或许可对搜索条件分组进行扩展
    /// </summary>
    public class SearchConditionItem
    {
        /// <summary>
        /// 条件
        /// </summary>
        public SearchTypeEnum ConditionType { get; set; }

        /// <summary>
        /// 多个where条件之间的关联关系
        /// </summary>
        public SearchWhereTypeEnum WhereType { get; set; }

        /// <summary>
        /// 查询字段名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 查询字段值
        /// </summary>
        public string Value { get; set; }
    }
}
