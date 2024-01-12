using NpgsqlTypes;
using vogue_decor.Domain.Interfaces;

namespace vogue_decor.Domain;

/// <summary>
/// Класс стиля товара
/// </summary>
public class Style : IBaseFilterItem
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public NpgsqlTsVector? SearchVector { get; set; }

    public List<Product> Products { get; set; } = new();
    public List<ProductStyle> ProductStyles { get; set; } = new();
}