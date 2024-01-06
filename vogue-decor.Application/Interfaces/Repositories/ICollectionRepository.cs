using vogue_decor.Application.DTOs.CollectionDTOs;
using vogue_decor.Application.DTOs.CollectionDTOs.ResponseDTOs;

namespace vogue_decor.Application.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория коллекций
    /// </summary>
    public interface ICollectionRepository
    {
        /// <summary>
        /// Получить список всех коллекций
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <param name="hostUrl">Домен API</param>
        /// <returns><see cref="GetCollectionsResponseDto"/></returns>
        Task<GetCollectionsResponseDto> GetAll(GetColectionsDto dto, string hostUrl);
        /// <summary>
        /// Получить коллекцию по идентификатору
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <param name="hostUrl">Домен API</param>
        /// <returns><see cref="CollectionLookupDto"/></returns>
        Task<CollectionLookupDto> GetById(GetCollectionByIdDto dto, string hostUrl);
        /// <summary>
        /// Получить коллекцию по названию
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <param name="hostUrl">Домен API</param>
        /// <returns><see cref="CollectionLookupDto"/></returns>
        Task<CollectionLookupDto> GetByName(GetCollectionByNameDto dto, string hostUrl);
        /// <summary>
        /// Добавить новую коллекцию
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <returns><see cref="AddCollectionResponseDto"/></returns>
        Task<AddCollectionResponseDto> Add(AddCollectionDto dto);
        /// <summary>
        /// Обновить детали коллекции
        /// </summary>
        /// <param name="dto">Входные данные</param>
        Task Update(UpdateCollectionDto dto);
        /// <summary>
        /// Удалить коллекцию
        /// </summary>
        /// <param name="dto">Входные данные</param>
        Task Delete(DeleteCollectionDto dto);
        /// <summary>
        /// Загрузить обложку коллекции
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <param name="webRootPath">Корневой путь проекта</param>
        /// <param name="hostUrl">Домен API</param>
        /// <returns><see cref="UploadPreviewResponseDto"/></returns>
        Task<UploadPreviewResponseDto> UploadPreview(UploadPreviewDto dto,
            string webRootPath, string hostUrl);
        /// <summary>
        /// Удалить обложку коллекции
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <param name="webRootPath">Корневой путь проекта</param>
        Task RemovePreview(RemovePreviewDto dto, string webRootPath);
        /// <summary>
        /// Добавить товар в коллекцию
        /// </summary>
        /// <param name="dto">Входные данные</param>
        Task AddProduct(AddProductToCollectionDto dto);
        /// <summary>
        /// Удалить товар из коллекции
        /// </summary>
        /// <param name="dto">Входные данные</param>
        Task DeleteProduct(DeleteProductFromCollectionDto dto);
        /// <summary>
        /// Установить порядок коллекции
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <returns></returns>
        Task SetIndex(SetCollectionIndexDto dto);
    }
}
