using System.ComponentModel;

namespace Server.Domain.Dto.InDto
{
    /// <summary>
    /// 分页传入对象
    /// </summary>
    public class PageInDto
    {
        /// <summary>
        /// 页索引
        /// </summary>
        [DefaultValue(1)]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 页数量
        /// </summary>
        [DefaultValue(20)]
        public int PageSize { get; set; } = 20;
    }
}
