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
    /// Минимальная цена
    /// </summary>
    public decimal? MinPrice { get; set; }
    /// <summary>
    /// Максимальная цена
    /// </summary>
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// Минимальная длина
    /// </summary>
    public decimal? MinLength { get; set; }
    /// <summary>
    /// Максимальная длина
    /// </summary>
    public decimal? MaxLength { get; set; }

    /// <summary>
    /// Минимальный диаметр
    /// </summary>
    public decimal? MinDiameter { get; set; }
    /// <summary>
    /// Максимальный диаметр
    /// </summary>
    public decimal? MaxDiameter { get; set; }

    /// <summary>
    /// Минимальная высота
    /// </summary>
    public decimal? MinHeight { get; set; }
    /// <summary>
    /// Максимальная высота
    /// </summary>
    public decimal? MaxHeight { get; set; }

    /// <summary>
    /// Минимальная ширина
    /// </summary>
    public decimal? MinWidth { get; set; }
    /// <summary>
    /// Максимальная ширина
    /// </summary>
    public decimal? MaxWidth { get; set; }

    /// <summary>
    /// Минимальный отступ
    /// </summary>
    public decimal? MinIndent { get; set; }
    /// <summary>
    /// Максимальный отступ
    /// </summary>
    public decimal? MaxIndent { get; set; }

    /// <summary>
    /// Минимальное количество лампочек
    /// </summary>
    public int? MinLampCount { get; set; }
    /// <summary>
    /// Максимальное количество лампочек
    /// </summary>
    public int? MaxLampCount { get; set; }

    /// <summary>
    /// Количество дополнительных параметров
    /// </summary>
    public Dictionary<int, FilterDto>? ChandelierTypes { get; set; }
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