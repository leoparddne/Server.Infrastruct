namespace Server.Infrastruct.Model.Models.Enums.Import
{
    /// <summary>
    /// 导入字段关联类型
    /// </summary>
    public enum ImpAssociationTypeEnum
    {
        /// <summary>
        /// 0:数据从上传文件中读取
        /// </summary>
        Common = 0,

        /// <summary>
        /// id字段-字段生成GUID
        /// </summary>
        GUID = 1,

        /// <summary>
        /// 外键字段-字段来自另一个表的某个字段-此时的字段需要来自所有的导入字段
        /// </summary>
        ForeignKey = 2,

        /// <summary>
        /// 搜索字段-字段需根据查询字段搜索后将查询结果后的其他字段回填-例如根据userNo查询得到结果后将userId回填
        /// </summary>
        SearchKey = 3,

        /// <summary>
        /// 获取用户No
        /// </summary>
        GetTokenUserNo = 4,


        /// <summary>
        /// 从文本中读取时间
        /// </summary>
        DateTime = 5,

        /// <summary>
        /// 默认值，保存在源表字段中
        /// </summary>
        DefaultValue = 6,
    }
}
