using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.BrandDTOs;

/// <summary>
/// DTO для удаления обложки бренда
/// </summary>
public class DeleteBrandImageDto
{
    /// <summary>
    /// Идентификатор бренда
    /// </summary>
    [Required(ErrorMessage = "Идентификатор бренда обязателен")]
    public Guid BrandId { get; set; }
    /// <summary>
    /// Название удаляемого файла
    /// </summary>
    [Required(ErrorMessage = "Название файла обязательно")]
    public string FileName { get; set; } = null!;
}