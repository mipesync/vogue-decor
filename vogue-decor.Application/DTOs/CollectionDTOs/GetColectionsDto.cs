namespace vogue_decor.Application.DTOs.CollectionDTOs;

/// <summary>
/// DTO для получения списка всех коллекций
/// </summary>
public class GetColectionsDto
{
    /// <summary>
    /// С какого количества элементов начинать выборку
    /// </summary>
    public int From { get; set; } = 0;

    /// <summary>
    /// Сколько элементов необходимо вывести
    /// </summary>
    public int Count { get; set; } = 10;
}