using vogue_decor.Application.Common.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для загрузки фотографии товара
    /// </summary>
    public class UploadImageDto
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        [Required(ErrorMessage = "Идентификатор товара обязателен")]
        public Guid ProductId { get; set; } = Guid.Empty;
        /// <summary>
        /// Список файлов с изображением
        /// </summary>
        [ExtensionValidator(Extensions = "jpg,jpeg,png,mp4")]
        public List<IFormFile>? Files { get; set; }
        /// <summary>
        /// Список ссылок на изображения
        /// </summary>
        public List<string>? Urls { get; set; }
    }
}
