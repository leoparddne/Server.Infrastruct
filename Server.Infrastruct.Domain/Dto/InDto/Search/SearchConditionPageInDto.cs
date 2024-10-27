namespace Server.Infrastruct.Model.Dto.InDto.Search
{
    /// <summary>
    /// 高级查询 分页
    /// </summary>
    public class SearchConditionPageInDto : PageInDto
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public List<SearchConditionItem> Conditions { get; set; }
    }
}
