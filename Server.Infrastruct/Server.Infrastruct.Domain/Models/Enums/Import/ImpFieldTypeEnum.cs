namespace Server.Infrastruct.Model.Models.Enums.Import
{
    /// <summary>
    /// 导入字段类型
    /// </summary>
    public enum ImpFieldTypeEnum
    {
        /// <summary>
        /// 不做任何处理 - 兼容旧数据
        /// </summary>
        Default = 0,

        /// <summary>
        /// 
        /// </summary>
        String = 1,

        /// <summary>
        /// 
        /// </summary>
        Float = 2,

        /// <summary>
        /// 
        /// </summary>
        Int = 3,

        /// <summary>
        /// 
        /// </summary>
        DateTime = 4
    }
}
