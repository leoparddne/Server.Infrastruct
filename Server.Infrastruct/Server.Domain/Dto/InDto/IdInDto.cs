using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Dto.InDto
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
        public string Id { get; set; }
    }
}
