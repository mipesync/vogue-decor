using vogue_decor.Domain;

namespace vogue_decor.Application.DTOs.FilterDTOs;

/// <summary>
/// DTO, возвращаемое из метода получения фильтров
/// </summary>
public class FiltersResponseDto
{
    /// <summary>
    /// Список цветов
    /// </summary>
    public List<ColorFilterLookup>? Colors { get; set; }
    /// <summary>
    /// Список типов люстр
    /// </summary>
    public List<FilterLookup>? ChandelierTypes { get; set; }
    /// <summary>
    /// Список типов товаров
    /// </summary>
    public List<FilterLookup>? ProductTypes { get; set; }
    /// <summary>
    /// Список категорий товара
    /// </summary>
    public List<CategoryFilterLookup>? Categories { get; set; }
    /// <summary>
    /// Список материалов товара
    /// </summary>
    public List<FilterLookup>? Materials { get; set; }
    /// <summary>
    /// Список стилей товара
    /// </summary>
    public List<FilterLookup>? Styles { get; set; }
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
    public string Name { get; set; } = null!;
}

public class ColorFilterLookup : FilterLookup
{
    /// <summary>
    /// Название цвета на английском
    /// </summary>
    public string EngName { get; set; } = null!;
}

public class CategoryFilterLookup : FilterLookup
{
    /// <summary>
    /// Идентификатор типа товара
    /// </summary>
    public int? ProductId { get; set; }
}