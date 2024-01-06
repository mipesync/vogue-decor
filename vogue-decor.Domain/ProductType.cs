using NpgsqlTypes;

namespace vogue_decor.Domain;

/// <summary>
/// Класс типа товара
/// </summary>
public class ProductType
{
    /// <summary>
    /// Идентификатор типа товара
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Название типа товара
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Поисковой вектор
    /// </summary>
    public NpgsqlTsVector SearchVector { get; set; }
}