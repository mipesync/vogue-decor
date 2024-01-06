using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для удаления товара
    /// </summary>
    public class DeleteProductDto
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        [Required(ErrorMessage = "Идентификатор товара обязателен")]
        public Guid ProductId { get; set; } = Guid.Empty;
    }
}
