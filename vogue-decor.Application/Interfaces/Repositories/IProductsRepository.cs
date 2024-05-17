using vogue_decor.Application.DTOs.ProductDTOs;
using vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs;

namespace vogue_decor.Application.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория товаров
    /// </summary>
    public interface IProductsRepository
    {
        /// <summary>
        /// Создать (добавить) новый товар
        /// </summary>
        /// <param name="dto">Входные данные для создания товара</param>
        /// <param name="webRootPath">Корневой путь проекта</param>
        /// <returns><see cref="CreateProductResponseDto"/></returns>
        Task<CreateProductResponseDto> CreateAsync(CreateProductDto dto, string webRootPath, string hostUrl);

        /// <summary>
        /// Импортировать список товаров из файла
        /// </summary>
        /// <param name="dto">Входные данные для импорта списка товаров</param>
        /// <param name="webRootPath">Корневой путь проекта</param>
        /// <returns><see cref="ImportProductsResponseDto"/></returns>
        Task<ImportProductsResponseDto> ImportAsync(ImportProductsDto dto, string webRootPath, string hostUrl);

        /// <summary>
        /// Обновить информацию о товаре
        /// </summary>
        /// <param name="dto">Входные данные для обновления информации о товаре</param>
        /// <returns></returns>
        Task UpdateAsync(UpdateProductDto dto);

        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="dto">Входные данные для удаления товара</param>
        /// <returns></returns>
        Task DeleteAsync(DeleteProductDto dto);

        /// <summary>
        /// Получить список всех товаров постранично
        /// </summary>
        /// <param name="dto">Входные данные для получения списка товаров постранично</param>
        /// <param name="hostUrl">Домен API</param>
        /// <returns><see cref="GetProductsResponseDto"/></returns>
        Task<GetProductsResponseDto> GetAllAsync(GetAllProductsDto dto, string hostUrl);

        /// <summary>
        /// Получить товар по его идентификатору
        /// </summary>
        /// <param name="dto">Входные данные для получения товара по идентификатору</param>
        /// <param name="hostUrl">Домен API</param>
        /// <returns><see cref="ProductResponseDto"/></returns>
        Task<ProductResponseDto> GetByIdAsync(GetProductByIdDto dto, string hostUrl);

        /// <summary>
        /// Получить список товаров по критериям
        /// </summary>
        /// <param name="dto">Входные данные для получения спичка товаров по критериям</param>
        /// <param name="hostUrl">Домен API</param>
        /// <returns><see cref="GetProductsByCriteriaResponseDto"/></returns>
        Task<GetProductsByCriteriaResponseDto> GetByCriteriaAsync(GetProductByCriteriaDto dto, string hostUrl);

        /// <summary>
        /// Получить товар по артикулу
        /// </summary>
        /// <param name="dto">Входные данные для получения товара по артикулу</param>
        /// <param name="hostUrl">Домен API</param>
        /// <returns><see cref="ProductResponseDto"/></returns>
        Task<GetProductsResponseDto> GetByArticleAsync(GetByArticleDto dto, string hostUrl);
        
        /// <summary>
        /// Получить товар по артикулу
        /// </summary>
        /// <param name="dto">Входные данные для получения товара по коду</param>
        /// <param name="hostUrl">Домен API</param>
        /// <returns><see cref="ProductResponseDto"/></returns>
        Task<GetProductsResponseDto> GetByCodeAsync(GetByCodeDto dto, string hostUrl);

        /// <summary>
        /// Получить список товаров по идентификатору коллекции
        /// </summary>
        /// <param name="dto">Входные данные для получения списка товаров по идентификатору коллекции</param>
        /// <param name="hostUrl">Домен API</param>
        /// <returns></returns>
        Task<GetProductsResponseDto> GetByCollectionIdAsync(GetProductByCollectionIdDto dto, string hostUrl);

        /// <summary>
        /// Найти альбомы по запросу
        /// </summary>
        /// <param name="dto">Входные данные для поиска альбомов по запросу</param>
        /// <param name="hostUrl">Домен API</param>
        /// <returns><see cref="GetProductsResponseDto"/></returns>
        Task<GetProductsResponseDto> SearchAsync(SearchProductDto dto, string hostUrl);

        /// <summary>
        /// Добавить товар в корзину
        /// </summary>
        /// <param name="dto">Входные данные для добавления товара в корзину</param>
        /// <returns></returns>
        Task AddToCartAsync(AddToCartDto dto);

        /// <summary>
        /// Удалить товар из корзины
        /// </summary>
        /// <param name="dto">Входные данные для удаления товара из корзины</param>
        /// <returns></returns>
        Task RemoveFromCartAsync(RemoveFromCartDto dto);
        
        /// <summary>
        /// Добавить товар в избранные
        /// </summary>
        /// <param name="dto">Входные данные для добавления товара в избранные</param>
        /// <returns></returns>
        Task AddToFavouriteAsync(AddToFavouriteDto dto);

        /// <summary>
        /// Удалить товар из избранных
        /// </summary>
        /// <param name="dto">Входные данные для удаления товара из избранных</param>
        /// <returns></returns>
        Task RemoveFromFavouriteAsync(RemoveFromFavouriteDto dto);

        /// <summary>
        /// Загрузить фотографию товара
        /// </summary>
        /// <param name="dto">Вхрдные данные</param>
        /// <param name="webRootPath">Корневой путь проекта</param>
        /// <param name="hostUrl">Домен API</param>
        /// <returns><see cref="UploadImageResponseDto"/></returns>
        Task<UploadImageResponseDto> UploadImageAsync(UploadImageDto dto, string webRootPath, string hostUrl);

        /// <summary>
        /// Удалить фотографию товара
        /// </summary>
        /// <param name="dto">Вхрдные данные</param>
        /// <param name="webRootPath">Корневой путь проекта</param>
        Task RemoveImageAsync(RemoveImageDto dto, string webRootPath);

        /// <summary>
        /// Изменить порядок файлов у товара
        /// </summary>
        /// <param name="dto">Входные данные</param>
        Task SetFileOrderAsync(SetFileOrderDto dto);

        /// <summary>
        /// Получить количество товара и фильтров по критериям
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <returns><see cref="GetFiltersCountResponseDto"/></returns>
        Task<GetFiltersCountResponseDto> GetFiltersCountAsync(GetProductByCriteriaDto dto);

        /// <summary>
        /// Изменить позицию товара
        /// </summary>
        /// <param name="productId">Идентификатор товара</param>
        /// <param name="index">Изменить позицию товара</param>
        Task SetIndexAsync(Guid productId, int index);

        /// <summary>
        /// Сравнить товары
        /// </summary>
        /// <param name="firstId">Идентификатор первого товара</param>
        /// <param name="secondId">Идентификатор второго товара</param>
        /// <returns></returns>
        Task<ProductsCompareResponseDto> CompareAsync(Guid firstId, Guid secondId);

        /// <summary>
        /// Обновить рейтинг товара
        /// </summary>
        /// <param name="dto">Входные данные</param>
        Task UpdateRating(UpdateRatingDto dto);
    }
}
