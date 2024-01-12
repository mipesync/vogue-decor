namespace vogue_decor.Domain.Interfaces;

/// <summary>
/// Интерфейс для элемента фильтра, предоставляющий поле с английским названием 
/// </summary>
public interface IEngName
{
    /// <summary>
    /// Английское название фильтра
    /// </summary>
    public string EngName { get; set; }
}