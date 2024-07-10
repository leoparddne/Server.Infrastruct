namespace Server.Domain.Models.ExceptionExtention
{
    /// <summary>
    /// 国际化异常值扩展
    /// </summary>
    public class LangageException : Exception
    {
        /// <summary>
        /// 国际化参数值
        /// </summary>
        public List<string> LangageValue { get; set; } = new();

        /// <summary>
        /// 国际化key
        /// </summary>
        public string LangageKey { get; set; }

        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="langageKey">国际化key</param>
        /// <param name="langageValue">国际化值</param>
        public LangageException(string langageKey, List<string> langageValue = null)
        {
            LangageKey = langageKey;
            LangageValue = langageValue;
        }
    }
}
