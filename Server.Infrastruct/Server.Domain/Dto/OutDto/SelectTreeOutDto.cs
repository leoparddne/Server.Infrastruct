namespace Server.Domain.Dto.OutDto
{
    /// <summary>
    /// 多级联动下拉框
    /// </summary>
    public class SelectTreeOutDto : SelectOutDto
    {
        /// <summary>
        /// 子级数据
        /// </summary>
        public List<SelectTreeOutDto> Data { get; set; } = new();
    }
}
