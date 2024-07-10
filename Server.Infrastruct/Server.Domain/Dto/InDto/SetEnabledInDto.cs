using Server.Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Dto.InDto
{
    /// <summary>
    /// 启用/禁用传入对象
    /// </summary>
    public class SetEnabledInDto
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "IdNullMsg")]
        public string Id { get; set; }

        /// <summary>
        /// 是否启用
        /// N--未启用
        /// Y--启用
        /// </summary>
        [YNValidationAttrbute(ErrorMessage = "IsEnabledErrorMsg")]
        public string IsEnabled { get; set; }
    }
}
