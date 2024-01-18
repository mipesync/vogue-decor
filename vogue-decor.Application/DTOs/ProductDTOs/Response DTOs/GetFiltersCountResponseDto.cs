namespace vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs;

public record GetFiltersCountResponseDto
{
    /// <summary>
    /// Количество цветов
    /// </summary>
    public Dictionary<int, FilterDto>? Colors { get; set; }

    /// <summary>
    /// Количество типов товара
    /// </summary>
    public Dictionary<int, FilterDto>? ProductTypes { get; set; }

    /// <summary>
    /// Количество категорий
    /// </summary>
    public Dictionary<int, FilterDto>? Categories { get; set; }

    /// <summary>
    /// Количество стилей
    /// </summary>
    public Dictionary<int, FilterDto>? Styles { get; set; }
    /// <summary>
    /// Диапазон цены
    /// </summary>
    public RangeFilterDto? Prices { get; set; }
    /// <summary>
    /// Диапазон длины
    /// </summary>
    public RangeFilterDto? Length { get; set; }
    /// <summary>
    /// Диапазон диаметра
    /// </summary>
    public RangeFilterDto? Diameter { get; set; }
    /// <summary>
    /// Диапазон высоты
    /// </summary>
    public RangeFilterDto? Height { get; set; }
    /// <summary>
    /// Диапазон ширины
    /// </summary>
    public RangeFilterDto? Width { get; set; }
    /// <summary>
    /// Диапазон отступа
    /// </summary>
    public RangeFilterDto? Indent { get; set; }
    /// <summary>
    /// Диапазон количества лампочек
    /// </summary>
    public RangeFilterDto? LampCount { get; set; }
    /// <summary>
    /// Количество дополнительных параметров
    /// </summary>
    public Dictionary<int, FilterDto>? AdditionalParams { get; set; }
    /// <summary>
    /// Количество материалов
    /// </summary>
    public Dictionary<int, FilterDto>? Materials { get; set; }
    /// <summary>
    /// Количество материалов рамки
    /// </summary>
    public Dictionary<int, FilterDto>? PictureMaterial { get; set; }
    /// <summary>
    /// Количество брендов
    /// </summary>
    public Dictionary<Guid, FilterDto>? Brands { get; set; }
    /// <summary>
    /// Количество коллекций
    /// </summary>
    public Dictionary<Guid, FilterDto>? Collections { get; set; }
    /// <summary>
    /// Общее количество товаров
    /// </summary>
    public int TotalCount { get; set; }

    public class FilterDto
    {
        /// <summary>
        /// Название фильтра
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Количество товаров с цветом 
        /// </summary>
        public int Count { get; set; }
    }
}