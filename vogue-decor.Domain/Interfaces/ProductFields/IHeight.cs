namespace vogue_decor.Domain.Interfaces.ProductFields;

/// <summary>
/// Интерфейс для товаров, имеющих высоту
/// </summary>
public interface IHeight
{
    /// <summary>
    /// Высота товара
    /// </summary>
    public decimal? Height { get; set; }
}