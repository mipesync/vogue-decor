using NpgsqlTypes;
using vogue_decor.Domain.Interfaces;

namespace vogue_decor.Domain;

/// <summary>
/// Класс материала товара
/// </summary>
public class Material : IBaseFilterItem
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public NpgsqlTsVector? SearchVector { get; set; }

    /// <summary>
    /// Список объектов товаров
    /// </summary>
    public List<Product> Products { get; set; } = new();
    public List<ProductMaterial> ProductMaterials { get; set; } = new();
}