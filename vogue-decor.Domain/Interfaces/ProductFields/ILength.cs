namespace vogue_decor.Domain.Interfaces.ProductFields;

/// <summary>
/// Интерфейс для товаров, имеющих длину
/// </summary>
public interface ILength
{
    /// <summary>
    /// Длина товара
    /// </summary>
    public decimal? Length { get; set; }
}