using NpgsqlTypes;
using vogue_decor.Domain.Interfaces;

namespace vogue_decor.Domain;

/// <summary>
/// Класс категории товара
/// </summary>
public class Category : IBaseFilterItem
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public NpgsqlTsVector? SearchVector { get; set; }
    /// <summary>
    /// Идентификатор типа товара
    /// </summary>
    public int? ProductTypeId { get; set; }

    /*/// <summary>
    /// Объект типа товара
    /// </summary>
    public ProductType ProductType { get; set; } = new();*/
}