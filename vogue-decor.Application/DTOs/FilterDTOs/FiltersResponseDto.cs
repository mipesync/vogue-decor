namespace vogue_decor.Application.DTOs.FilterDTOs;

/// <summary>
/// DTO, возвращаемое из метода получения фильтров
/// </summary>
public class FiltersResponseDto
{
    /// <summary>
    /// Список цветов
    /// </summary>
    public List<ColorFilterLookup> Colors { get; set; } = new();
    /// <summary>
    /// Список типов люстр
    /// </summary>
    public List<FilterLookup> ChandelierTypes { get; set; } = new();
    /// <summary>
    /// Список типов товаров
    /// </summary>
    public List<FilterLookup> ProductTypes { get; set; } = new();
}

public class FilterLookup
{
    /// <summary>
    /// Идентификатор фильтра
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Название фильтра
    /// </summary>
    public string Name { get; set; }
}

public class ColorFilterLookup : FilterLookup
{
    /// <summary>
    /// Название цвета на английском
    /// </summary>
    public string EngName { get; set; }
}