using vogue_decor.Application.DTOs.FilterDTOs;

namespace vogue_decor.Application.Interfaces.Repositories;

/// <summary>
/// Интерфейс репозитория фильтров
/// </summary>
public interface IFiltersRepository
{
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
    /// Получить список всех фильтров
    /// </summary>
    /// <returns><see cref="FiltersResponseDto"/></returns>
    Task<FiltersResponseDto> GetFilters();
}