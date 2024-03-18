using vogue_decor.Domain.Enums;

namespace vogue_decor.Application.DTOs.ProductDTOs;

/// <summary>
/// DTO для отбора товаров по критериям
/// </summary>
public class GetProductByCriteriaDto
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Выбранные цвета
    /// </summary>
    public int[]? Colors { get; set; }
    /// <summary>
    /// Выбранные типы товара
    /// </summary>
    public int[]? ProductTypes { get; set; }
    /// <summary>
    /// Выбранные категории
    /// </summary>
    public int[]? Categories { get; set; }
    /// <summary>
    /// Выбранные стили
    /// </summary>
    public int[]? Styles { get; set; }
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
    /// Выбранные дополнительные параметры
    /// </summary>
    public int[]? AdditionalParams { get; set; }
    /// <summary>
    /// Выбранные материалы
    /// </summary>
    public int[]? Materials { get; set; }
    /// <summary>
    /// Выбранные материалы рамки
    /// </summary>
    public int[]? PictureMaterial { get; set; }
    /// <summary>
    /// С какого количества начать выборку товаров (по умолчанию - 0)
    /// </summary>
    public int From { get; set; } = 0;
    /// <summary>
    /// Выбранные товара, которое надо получить (по умолчанию - 10)
    /// </summary>
    public int Count { get; set; } = 10;
    /// <summary>
    /// Участие товаров в распродаже
    /// </summary>
    public bool? IsSale { get; set; }
    /// <summary>
    /// Тип сортировки
    /// </summary>
    public SortTypes? SortType { get; set; }
    /// <summary>
    /// Выбранные бренды
    /// </summary>
    public Guid[]? BrandsId { get; set; }
    /// <summary>
    /// Выбранные коллекции
    /// </summary>
    public Guid[]? CollectionsId { get; set; }
}