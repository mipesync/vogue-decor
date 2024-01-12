namespace vogue_decor.Domain.Interfaces.ProductFields;

/// <summary>
/// Интерфейс для товаров, имеющих лампочки
/// </summary>
public interface ILampCount
{
    /// <summary>
    /// Количество лампочек
    /// </summary>
    public int LampCount { get; set; }
}