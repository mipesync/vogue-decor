namespace vogue_decor.Domain;

public class ProductMaterial
{
    /// <summary>
    /// Идентификатор товара
    /// </summary>
    public Guid ProductId { get; set; }
    /// <summary>
    /// Идентификатор материала
    /// </summary>
    public int MaterialId { get; set; }

    /// <summary>
    /// Объект товара
    /// </summary>
    public Product Product { get; set; } = new();
    /// <summary>
    /// Объект материала
    /// </summary>
    public Material Material { get; set; } = new();
}