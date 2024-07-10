using Server.Domain.Attributes;

namespace Server.Domain.Dto.InDto
{
    public class SetListEnabledInDto
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        [YNValidationAttrbute(ErrorMessage = "IsEnabledErrorMsg")]
        public string IsEnabled { get; set; }

        public List<string> Ids { get; set; } = new();
    }
}
