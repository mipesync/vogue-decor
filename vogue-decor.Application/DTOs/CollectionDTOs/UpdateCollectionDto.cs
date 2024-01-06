using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.CollectionDTOs
{
    /// <summary>
    /// DTO для обновления деталей коллекции
    /// </summary>
    public class UpdateCollectionDto
    {
        /// <summary>
        /// Идентификатор коллекции
        /// </summary>
        [Required(ErrorMessage = "Идентификатор коллекции обязателен")]
        public Guid CollectionId { get; set; } = Guid.Empty;
        /// <summary>
        /// Новое название коллекции
        /// </summary>
        [Required(ErrorMessage = "Название обязательно")]
        public string Name { get; set; } = string.Empty;
    }
}
