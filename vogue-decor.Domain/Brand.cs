using vogue_decor.Domain.Interfaces;

namespace vogue_decor.Domain;

/// <summary>
/// Класс бренда
/// </summary>
public class Brand : ISecondaryEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Url { get; set; } = null!;
    
    /// <summary>
    /// Список товаров бренда
    /// </summary>
    public List<Product> Products { get; set; } = new();
    /// <summary>
    /// Список коллекций бренда
    /// </summary>
    public List<Collection> Collections { get; set; } = new();
}