using NpgsqlTypes;
using vogue_decor.Domain.Interfaces;

namespace vogue_decor.Domain;

/// <summary>
/// Класс типа товара
/// </summary>
public class ProductType : IBaseFilterItem
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public NpgsqlTsVector? SearchVector { get; set; }

    /*/// <summary>
    /// Список категорий
    /// </summary>
    public List<Category> Categories { get; set; } = new();*/
}