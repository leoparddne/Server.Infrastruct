namespace Server.Infrastruct.Model.Models.Enums.Import
{
    /// <summary>
    /// 导入状态
    /// </summary>
    public enum ImportStatesEnum
    {
        /// <summary>
        /// 导入成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 失败
        /// </summary>
        Faild = 1,

        /// <summary>
        /// 可以导入
        /// </summary>
        CanImport = 2,

        /// <summary>
        /// 不能为空
        /// </summary>
        NotNull = 3,

        /// <summary>
        /// 重复
        /// </summary>
        Duplicate = 4,

        /// <summary>
        /// 字段类型不匹配
        /// </summary>
        FieldTypeNotMatch = 5,

        /// <summary>
        /// 待导入
        /// </summary>
        WaitImport = 10

    }
}
