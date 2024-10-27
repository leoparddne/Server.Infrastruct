using System.ComponentModel.DataAnnotations;

namespace Server.Infrastruct.Model.Dto.InDto
{
    /// <summary>
    /// Id传入对象
    /// </summary>
    public class IdInDto
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "IdNullMsg")]
        public string ID { get; set; }
    }
}
