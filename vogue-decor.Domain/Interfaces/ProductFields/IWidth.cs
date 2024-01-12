namespace vogue_decor.Domain.Interfaces.ProductFields;

/// <summary>
/// Интерфейс для товаров, имеющих ширину
/// </summary>
public interface IWidth
{
    /// <summary>
    /// Ширина товара
    /// </summary>
    public decimal? Width { get; set; }
}