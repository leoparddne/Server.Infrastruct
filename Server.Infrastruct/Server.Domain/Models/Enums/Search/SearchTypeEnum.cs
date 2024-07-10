namespace Server.Domain.Models.Enums.Search
{
    /// <summary>
    /// 搜索条件类型
    /// </summary>
    public enum SearchTypeEnum
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 0,

        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual = 1,

        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan = 2,

        /// <summary>
        /// 大于大于
        /// </summary>
        GreaterThanOrEqual = 3,

        /// <summary>
        /// 小于
        /// </summary>
        LessThan = 4,

        /// <summary>
        /// 小于等于
        /// </summary>
        LessThanOrEqual = 5,

        /// <summary>
        /// 包含
        /// </summary>
        Container = 6,

        /// <summary>
        /// 左包含
        /// </summary>
        ContainerLeft = 7,

        /// <summary>
        /// 右包含
        /// </summary>
        ContainerRight = 8,
    }
}
