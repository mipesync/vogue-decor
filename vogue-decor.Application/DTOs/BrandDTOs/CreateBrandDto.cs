using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using vogue_decor.Application.Common.Attributes;

namespace vogue_decor.Application.DTOs.BrandDTOs;

/// <summary>
/// DTO для создания бренда
/// </summary>
public class CreateBrandDto
{
    /// <summary>
    /// Название бренда
    /// </summary>
    [Required(ErrorMessage = "Название бренда обязательно")]
    public string Name { get; set; } = null!;
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