using vogue_decor.Application.DTOs.ProductDTOs;
using vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs;

namespace vogue_decor.Application.DTOs.FilesDTOs.ResponseDTOs;

/// <summary>
/// DTO, возвращаемое их метода получения всех файлов
/// </summary>
public class GetAllFilesResponseDto
{
    /// <summary>
    /// Список файлов
    /// </summary>
    public List<FileDto> Files { get; set; } = new();
}