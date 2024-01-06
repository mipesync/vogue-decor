using vogue_decor.Application.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Http;
using vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs;

namespace vogue_decor.Application.DTOs.FilesDTOs.ResponseDTOs;

/// <summary>
/// DTO, возвращаемое из метода загрузки файла
/// </summary>
public class UploadFileResponseDto
{
    /// <summary>
    /// Ссылки на файл
    /// </summary>
    public List<FileDto> Urls { get; set; } = new();
}