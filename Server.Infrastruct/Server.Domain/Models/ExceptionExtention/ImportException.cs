using Server.Domain.Dto.OutDto;

namespace Server.Domain.Models.ExceptionExtention
{
    /// <summary>
    /// 导入异常返回
    /// </summary>
    public class ImportException : Exception
    {
        /// <summary>
        /// 数据
        /// </summary>
        public List<LangageExceptionOutDto> DataList { get; set; } = new List<LangageExceptionOutDto>();

        /// <summary>
        /// 添加异常数据
        /// </summary>
        /// <param name="check"></param>
        /// <param name="model"></param>
        public void CheckError(bool check, LangageExceptionOutDto model)
        {
            if (check)
            {
                DataList.Add(model);
            }
        }
    }
}
