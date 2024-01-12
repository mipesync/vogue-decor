namespace vogue_decor.Domain;

/// <summary>
/// Класс бренда
/// </summary>
public class Brand
{
    /// <summary>
    /// Идентификатор бренда
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Название бренда
    /// </summary>
    public string Name { get; set; } = null!;
    /// <summary>
    /// Ссылка на обложку бренда
    /// </summary>
    public string Url { get; set; } = null!;
    
    /// <summary>
    /// Список товаров бренда
    /// </summary>
    public List<Product> Products { get; set; } = new();
}