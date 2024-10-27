namespace Server.Infrastruct.Model.Dto.OutDto
{
    /// <summary>
    /// 国际化多参数值异常返回
    /// </summary>
    public class LangageExceptionOutDto
    {
        /// <summary>
        /// 国际化key
        /// </summary>
        public string LangageKey { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public int Row { get; set; }
        /// <summary>
        ///列 
        /// </summary>
        public int Col { get; set; }
        /// <summary>
        ///列名
        /// </summary>
        public string ColName { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public List<string> Parameters { get; set; }

        /// <summary>
        /// 初始化异常
        /// </summary>
        /// <param name="langageKey"></param>
        /// <param name="parameters"></param>
        public LangageExceptionOutDto(string langageKey, List<string> parameters)
        {
            LangageKey = langageKey;
            Parameters = parameters;
        }
    }
}
