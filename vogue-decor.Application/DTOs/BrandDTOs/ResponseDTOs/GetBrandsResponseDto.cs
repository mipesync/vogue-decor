namespace vogue_decor.Application.DTOs.BrandDTOs.ResponseDTOs;

/// <summary>
/// DTO со списком брендов
/// </summary>
public class GetBrandsResponseDto
{
    /// <summary>
    /// Список брендов
    /// </summary>
    public List<BrandResponseDto> Brands { get; set; } = new();
    /// <summary>
    /// Суммарное количество брендов
    /// </summary>
    public int Total { get; set; }
}