namespace vogue_decor.Domain.Interfaces.ProductFields;

/// <summary>
/// Интерфейс для товаров, имеющих отступ от стены
/// </summary>
public interface IIndent
{
    /// <summary>
    /// Отступ от стены
    /// </summary>
    public decimal? Indent { get; set; }
}