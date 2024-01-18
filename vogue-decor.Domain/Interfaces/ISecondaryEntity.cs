namespace vogue_decor.Domain.Interfaces;

/// <summary>
/// Интерфейс для второстепенной сущности
/// </summary>
public interface ISecondaryEntity
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Название сущности
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Ссылка на обложку сущности
    /// </summary>
    public string Url { get; set; }
}