using vogue_decor.Application.Common.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для импорта списка товаров
    /// </summary>
    public class ImportProductsDto
    {
        /// <summary>
        /// Файл, содержащий список товаров
        /// </summary>
        [Required(ErrorMessage = "Файл обязателен")]
        [ExtensionValidator(Extensions = ".xml", ErrorMessage = "Разрешён только .xml формат файла")]
        public IFormFile File { get; set; } = null!;
    }
}
