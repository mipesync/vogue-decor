using NpgsqlTypes;

namespace vogue_decor.Domain.Interfaces;

/// <summary>
/// Базовый интерфейс для элемента фильтра  
/// </summary>
public interface IBaseFilterItem
{
    /// <summary>
    /// Идентификатор фильтра
    /// </summary>
    int Id { get; set; }
    /// <summary>
    /// Название фильтра
    /// </summary>
    string Name { get; set; }
    /// <summary>
    /// Вектор поиска
    /// </summary>
    public NpgsqlTsVector? SearchVector { get; set; }
}