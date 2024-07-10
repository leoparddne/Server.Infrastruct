using Server.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public class YNValidationAttrbute : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            if (value.ToString() == StatesEnum.Y.ToString() || value.ToString() == StatesEnum.N.ToString())
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class YNNullValidationAttribute : YNValidationAttrbute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (string.IsNullOrWhiteSpace(value.ToString())) return true;
            return base.IsValid(value);
        }
    }
}
