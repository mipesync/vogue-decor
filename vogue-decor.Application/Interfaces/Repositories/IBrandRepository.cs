using vogue_decor.Application.DTOs.BrandDTOs;
using vogue_decor.Application.DTOs.BrandDTOs.ResponseDTOs;
using vogue_decor.Application.DTOs.CollectionDTOs;
using vogue_decor.Application.DTOs.CollectionDTOs.ResponseDTOs;

namespace vogue_decor.Application.Interfaces.Repositories;

/// <summary>
/// Интерфейс репозитория бренда
/// </summary>
public interface IBrandRepository
{
    /// <summary>
    /// Создать бренд
    /// </summary>
    /// <param name="dto">Входные данные</param>
    /// <param name="webRootPath">Корневой путь проекта</param>
    /// <param name="hostUrl">Домен API</param>
    /// <returns><see cref="CreateBrandResponseDto"/></returns>
    Task<CreateBrandResponseDto> CreateAsync(CreateBrandDto dto, string webRootPath, string hostUrl);
    /// <summary>
    /// Обновить бренд
    /// </summary>
    /// <param name="dto">Входные данные</param>
    /// <param name="webRootPath">Корневой путь проекта</param>
    /// <param name="hostUrl">Домен API</param>
    Task UpdateAsync(UpdateBrandDto dto, string webRootPath, string hostUrl);
    /// <summary>
    /// Удалить бренд
    /// </summary>
    /// <param name="webRootPath">Корневой путь проекта</param>
    /// <param name="brandId">Идентификатор удаляемого бренда</param>
    Task DeleteAsync(Guid brandId, string webRootPath);
    /// <summary>
    /// Удалить обложку бренда
    /// </summary>
    /// <param name="dto">Входные данные</param>
    /// <param name="webRootPath">Корневой путь проекта</param>
    /// <returns></returns>
    Task DeleteImageAsync(DeleteBrandImageDto dto, string webRootPath);
    /// <summary>
    /// Получить список всех брендов
    /// </summary>
    /// <param name="skip">Количество брендов, которое необходимо пропустить</param>
    /// <param name="take">Количество брендов, которое необходимо получить</param>
    /// <param name="hostUrl">Домен API</param>
    /// <returns><see cref="GetBrandsResponseDto"/></returns>
    Task<GetBrandsResponseDto> GetAllAsync(string hostUrl, int skip, int take);
    /// <summary>
    /// Получить бренд по идентификатору
    /// </summary>
    /// <param name="brandId">Идентификатор бренда</param>
    /// <param name="hostUrl">Домен API</param>
    /// <returns><see cref="BrandResponseDto"/></returns>
    Task<BrandResponseDto> GetByIdAsync(Guid brandId, string hostUrl);
    /// <summary>
    /// Получить список коллекций по идентификатору бренда
    /// </summary>
    /// <param name="dto">Входные данные</param>
    /// <param name="hostUrl">Домен API</param>
    /// <returns><see cref="GetCollectionsResponseDto"/></returns>
    Task<GetCollectionsResponseDto> GetCollectionsAsync(GetCollectionsByBrandIdDto dto, string hostUrl);
}