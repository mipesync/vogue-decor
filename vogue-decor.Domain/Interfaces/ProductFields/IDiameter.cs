namespace vogue_decor.Domain.Interfaces.ProductFields;

/// <summary>
/// Интерфейс для товаров, имеющих диаметр
/// </summary>
public interface IDiameter
{
    /// <summary>
    /// Диаметр товара
    /// </summary>
    public decimal? Diameter { get; set; }
}