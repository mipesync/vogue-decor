namespace vogue_decor.Domain.Interfaces.ProductFields;

/// <summary>
/// Интерфейс для товара, у которого есть материал рамки
/// </summary>
public interface IPictureMaterial
{
    /// <summary>
    /// Список материалов рамки
    /// </summary>
    public int[]? PictureMaterial { get; set; }
}