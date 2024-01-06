using vogue_decor.Application.Common.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.CollectionDTOs
{
    /// <summary>
    /// DTO для загрузки фотографии товара
    /// </summary>
    public class UploadPreviewDto
    {
        /// <summary>
        /// Идентификатор коллекции
        /// </summary>
        [Required(ErrorMessage = "Идентификатор коллекции обязателен")]
        public Guid CollectionId { get; set; } = Guid.Empty;
        /// <summary>
        /// Файл с изображением
        /// </summary>
        [ExtensionValidator(Extensions = "jpg,jpeg,png")]
        public IFormFile? File { get; set; }
        /// <summary>
        /// Ссылка на изображение
        /// </summary>
        public string? Url { get; set; }
    }
}
