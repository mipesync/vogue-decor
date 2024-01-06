using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.CollectionDTOs
{
    /// <summary>
    /// DTO для удаления товара из коллекции
    /// </summary>
    public class DeleteProductFromCollectionDto
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
