using NpgsqlTypes;

namespace vogue_decor.Domain.Interfaces.ProductFields;

/// <summary>
/// Базовый интерфейс для товара
/// </summary>
public interface IBaseProduct
{
    /// <summary>
    /// Идентификатор товара
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Название товара
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Описание товара
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Тип товара
    /// </summary>
    public int[] Types { get; set; }
    /// <summary>
    /// Артикул товара
    /// </summary>
    public string Article { get; set; }
    /// <summary>
    /// Код товара
    /// </summary>
    public string Code { get; set; }
    /// <summary>
    /// Цена товара
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// Список цветов товара
    /// </summary>
    public int[] Colors { get; set; } 
    /// <summary>
    /// Скидка на товар
    /// </summary>
    /// <remarks>Указывается в процентах</remarks>
    public uint? Discount { get; set; }
    /// <summary>
    /// Рейтинг товара
    /// </summary>
    public decimal? Rating { get; set; }
    /// <summary>
    /// Стиль товара
    /// </summary>
    public int[]? Styles { get; set; }
    /// <summary>
    /// Материал товара
    /// </summary>
    public int[]? Materials { get; set; }
    /// <summary>
    /// Наличие товара
    /// </summary>
    public int Availability { get; set; }
    /// <summary>
    /// Идентификатор бренда
    /// </summary>
    public Guid BrandId { get; set; }
    /// <summary>
    /// Идентификатор колекции
    /// </summary>
    public Guid? CollectionId { get; set; }
    /// <summary>
    /// Список ссылок на изображения
    /// </summary>
    public List<string> Urls { get; set; }
    /// <summary>
    /// Вектор поиска
    /// </summary>
    public NpgsqlTsVector? SearchVector { get; set; }
}