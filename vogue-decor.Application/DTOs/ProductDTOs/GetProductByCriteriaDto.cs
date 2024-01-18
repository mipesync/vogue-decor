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