using vogue_decor.Application.DTOs.FilesDTOs;
using vogue_decor.Application.DTOs.FilesDTOs.ResponseDTOs;
using vogue_decor.Application.DTOs.ProductDTOs;

namespace vogue_decor.Application.Interfaces.Repositories;

/// <summary>
/// Интерфейс, описывающий методы взаимодействия с файлами
/// </summary>
public interface IFilesRepository
{
    /// <summary>
    /// Загрузить файлы
    /// </summary>
    /// <returns><see cref="UploadFileResponseDto"/></returns>
    Task<UploadFileResponseDto> Upload(UploadFileDto dto, string webRootPath, string hostUrl);

    /// <summary>
    /// Удалить файл
    /// </summary>
    /// <param name="fileName">Название файла</param>
    /// <param name="webRootPath">Корневой путь проекта</param>
    Task Delete(string fileName, string webRootPath);

    /// <summary>
    /// Получить список фотографий
    /// </summary>
    /// <param name="dto">Входные данные</param>
    /// <param name="hostUrl"></param>
    /// <returns></returns>
    Task<GetAllFilesResponseDto> GetAll(GetAllFilesDto dto, string hostUrl);
}