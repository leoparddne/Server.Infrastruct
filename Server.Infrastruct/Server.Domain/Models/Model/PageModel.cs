namespace Server.Domain.Models.Model
{
    /// <summary>
    /// 分页对象
    /// </summary>
    public class PageModel<T>
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> DataList { get; set; }
    }
}
