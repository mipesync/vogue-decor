namespace vogue_decor.Domain.Interfaces.ProductFields;

/// <summary>
/// Интерфейс для товаров, содержащих список типов люстр
/// </summary>
public interface IChandelierType
{
    /// <summary>
    /// Список типов люстр
    /// </summary>
    public int[]? ChandelierTypes { get; set; }
}