using Server.Infrastruct.Model.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastruct.Model.Dto.InDto
{
    /// <summary>
    /// 启用/禁用传入对象
    /// </summary>
    public class SetEnabledInDto: IdInDto
    {

        /// <summary>
        /// 是否启用
        /// N--未启用
        /// Y--启用
        /// </summary>
        [YNValidationAttrbute(ErrorMessage = "IsEnabledErrorMsg")]
        public string IsEnabled { get; set; }
    }
}
