using NpgsqlTypes;
using vogue_decor.Domain.Interfaces;

namespace vogue_decor.Domain;

/// <summary>
/// Класс типа люстры
/// </summary>
public class ChandelierType : IBaseFilterItem
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public NpgsqlTsVector? SearchVector { get; set; }
}