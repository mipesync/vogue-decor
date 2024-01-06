using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.CollectionDTOs
{
    /// <summary>
    /// DTO для получения коллекции по идентификатору
    /// </summary>
    public class GetCollectionByIdDto
    {
        /// <summary>
        /// Идентификатор коллекции
        /// </summary>
        [Required(ErrorMessage = "Идентификатор коллекции обязателен")]
        public Guid CollectionId { get; set; } = Guid.Empty;
    }
}
