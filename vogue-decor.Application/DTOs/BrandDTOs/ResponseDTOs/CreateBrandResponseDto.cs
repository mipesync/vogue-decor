namespace vogue_decor.Application.DTOs.BrandDTOs.ResponseDTOs;

/// <summary>
/// DTO, возвращаемое из метода создания бренда
/// </summary>
public class CreateBrandResponseDto
{
    /// <summary>
    /// Идентификатор созданного бренда
    /// </summary>
    public Guid BrandId { get; set; }
}