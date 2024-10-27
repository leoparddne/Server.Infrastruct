using Server.Infrastruct.Model.Attributes;
using Server.Infrastruct.Model.Models.Enums;

namespace Server.Infrastruct.Model.Dto.InDto
{
    public class SetListEnabledInDto
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        [YNValidationAttrbute(ErrorMessage = "IsEnabledErrorMsg")]
        public StatesEnum IsEnabled { get; set; }


        public List<string> IDs { get; set; } = new();
    }
}
