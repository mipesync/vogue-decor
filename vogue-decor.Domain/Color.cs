using NpgsqlTypes;

namespace vogue_decor.Domain;

/// <summary>
/// Класс цвета
/// </summary>
public class Color
{
    /// <summary>
    /// Идентификатор цвета
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Название цвета на русском
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Название цвета на английском
    /// </summary>
    /// <returns></returns>
    public string EngName { get; set; }
    /// <summary>
    /// Поисковой вектор
    /// </summary>
    public NpgsqlTsVector SearchVector { get; set; }
}