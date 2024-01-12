namespace vogue_decor.Domain;

public class ProductStyle
{
    /// <summary>
    /// Идентификатор товара
    /// </summary>
    public Guid ProductId { get; set; }
    /// <summary>
    /// Идентификатор стиля
    /// </summary>
    public int StyleId { get; set; }

    /// <summary>
    /// Объект товара
    /// </summary>
    public Product Product { get; set; } = new();
    /// <summary>
    /// Объект стиля
    /// </summary>
    public Style Style { get; set; } = new();
}