using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using vogue_decor.Application.Common.Attributes;

namespace vogue_decor.Application.DTOs.BrandDTOs;

/// <summary>
/// DTO для обновления бренда
/// </summary>
public class UpdateBrandDto
{
    /// <summary>
    /// Идентификатор бренда
    /// </summary>
    [Required(ErrorMessage = "Идентификатор бренда обязателен")]
    public Guid BrandId { get; set; }
    /// <summary>
    /// Название бренда
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Файл обложки бренда
    /// </summary>
    [ExtensionValidator(Extensions = "jpg,jpeg,png,mp4")]
    public IFormFile? File { get; set; }
    /// <summary>
    /// Ссылка на обложку бренда
    /// </summary>
    public string? Url { get; set; }
}