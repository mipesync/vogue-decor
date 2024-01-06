using Microsoft.AspNetCore.Http;

namespace vogue_decor.Application.DTOs.FilesDTOs;

/// <summary>
/// DTO ля загрузки изображений в хранилище
/// </summary>
public class UploadFileDto
{
    /// <summary>
    /// Файлы для загрузки
    /// </summary>
    public List<IFormFile> Files { get; set; } = new();

    /// <summary>
    /// Ссылки на файлы для загрузки
    /// </summary>
    public List<string> Urls { get; set; } = new();
}