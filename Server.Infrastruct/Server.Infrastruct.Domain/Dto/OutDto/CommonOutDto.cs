namespace Server.Infrastruct.Model.Dto.OutDto
{
    public class CommonOutDto
    {
        public string? CreateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdateUser { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 通用ID字段
        /// </summary>
        public string? ID { get; set; }
    }
}
