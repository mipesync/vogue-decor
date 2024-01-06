using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для удаления фотографии товара
    /// </summary>
    public class RemoveImageDto
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        [Required(ErrorMessage = "Идентификатор товара обязателен")]
        public Guid ProductId { get; set; } = Guid.Empty;
        /// <summary>
        /// Название файла, который нужно удалить
        /// </summary>
        [Required(ErrorMessage = "Название файла обязательно")]
        public string FileName { get; set; } = string.Empty;
    }
}
