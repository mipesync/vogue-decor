using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.CollectionDTOs
{
    /// <summary>
    /// DTO для добавления товара в коллекцию
    /// </summary>
    public class AddProductToCollectionDto
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        [Required(ErrorMessage = "Идентификатор товара обязателен")]
        public Guid ProductId { get; set; } = Guid.Empty;
        /// <summary>
        /// Идентификатор коллекции
        /// </summary>
        [Required(ErrorMessage = "Идентификатор коллекции обязателен")]
        public Guid CollectionId { get; set; } = Guid.Empty;
    }
}
