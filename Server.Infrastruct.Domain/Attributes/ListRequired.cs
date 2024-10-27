using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Server.Infrastruct.Model.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public class ListRequired : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            if (value is IList data)
            {
                return data.Count > 0;
            }

            return false;
        }

    }
}
