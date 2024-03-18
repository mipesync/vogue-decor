using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using vogue_decor.Application.Common.Attributes;

namespace vogue_decor.Application.DTOs.CollectionDTOs
{
    /// <summary>
    /// DTO для добавления новой коллекции
    /// </summary>
    public class AddCollectionDto
    {
        /// <summary>
        /// Название коллекции
        /// </summary>
        [Required(ErrorMessage = "Название обязательно")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Идентификатор бренда
        /// </summary>
        [Required(ErrorMessage = "Идентификатор коллекции обязателен")]
        public Guid BrandId { get; set; }
        /// <summary>
        /// Файл обложки коллекции
        /// </summary>
        [ExtensionValidator(Extensions = "jpg,jpeg,png,mp4")]
        public IFormFile? File { get; set; }
        /// <summary>
        /// Ссылка на обложку коллеции
        /// </summary>
        public string? Url { get; set; }
    }
}
