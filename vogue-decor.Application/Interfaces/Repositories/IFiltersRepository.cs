using vogue_decor.Application.DTOs.FilterDTOs;

namespace vogue_decor.Application.Interfaces.Repositories;

/// <summary>
/// Интерфейс репозитория фильтров
/// </summary>
public interface IFiltersRepository
{
    /// <summary>
    /// Получить список всех фильтров
    /// </summary>
    /// <returns><see cref="FiltersResponseDto"/></returns>
    Task<FiltersResponseDto> GetFilters();
    /// <summary>
    /// Добавить цвет
    /// </summary>
    /// <param name="name">Название цвета на русском</param>
    /// <param name="engName">Название цвета на английском</param>
    Task AddColor(string name, string engName);
    /// <summary>
    /// Удалить цвет
    /// </summary>
    /// <param name="id">Идентифкатор цвета</param>
    /// <returns></returns>
    Task DeleteColor(int id);
    /// <summary>
    /// Добавить тип товара
    /// </summary>
    /// <param name="name">Название типа товара</param>
    Task AddProductType(string name);
    /// <summary>
    /// Удалить тип товара
    /// </summary>
    /// <param name="id">Идентификатор типа товара</param>
    Task DeleteProductType(int id);
    /// <summary>
    /// Добавить тип люстры
    /// </summary>
    /// <param name="name">Название типа люстры</param>
    Task AddChandelierType(string name);
    /// <summary>
    /// Удалить тип люстры
    /// </summary>
    /// <param name="id">Идентификатор типа люстры</param>
    Task DeleteChandelierType(int id);
    /// <summary>
    /// Добавить категорию
    /// </summary>
    /// <param name="name">Название категории</param>
    /// <param name="productTypeId">Идентификатор типа товара</param>
    Task AddCategory(string name, int productTypeId);
    /// <summary>
    /// Удалить категорию
    /// </summary>
    /// <param name="id">Идентификатор категории</param>
    Task DeleteCategory(int id);
    /// <summary>
    /// Добавить материал
    /// </summary>
    /// <param name="name">Название материала</param>
    Task AddMaterial(string name);
    /// <summary>
    /// Удалить материал
    /// </summary>
    /// <param name="id">Идентификатор материала</param>
    Task DeleteMaterial(int id);
    /// <summary>
    /// Добавить стиль
    /// </summary>
    /// <param name="name">Название стиля</param>
    Task AddStyle(string name);
    /// <summary>
    /// Удалить стиль
    /// </summary>
    /// <param name="id">Идентификатор стиля</param>
    Task DeleteStyle(int id);
}