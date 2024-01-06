using NpgsqlTypes;

namespace vogue_decor.Domain;

/// <summary>
/// Класс типов люстр
/// </summary>
public class ChandelierType
{
    /// <summary>
    /// Идентификатор типа люстры
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Название типа люстры
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Поисковой вектор
    /// </summary>
    public NpgsqlTsVector SearchVector { get; set; }
}