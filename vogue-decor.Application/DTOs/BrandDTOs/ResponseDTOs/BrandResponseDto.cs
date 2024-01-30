using vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs;

namespace vogue_decor.Application.DTOs.BrandDTOs.ResponseDTOs;

/// <summary>
/// DTO информации о бренде
/// </summary>
public class BrandResponseDto
{
    /// <summary>
    /// Идентификатор бренда
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Название бренда
    /// </summary>
    public string Name { get; set; } = null!;
    /// <inheritdoc cref="FileDto"/>
    public FileDto File { get; set; } = new();
}