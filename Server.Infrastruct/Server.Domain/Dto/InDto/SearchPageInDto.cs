namespace Server.Domain.Dto.InDto
{
    /// <summary>
    /// 分页查询字段
    /// </summary>
    public class SearchPageInDto : PageInDto
    {
        /// <summary>
        /// 查询字段
        /// </summary>
        public string SearchText { get; set; }
    }
}
