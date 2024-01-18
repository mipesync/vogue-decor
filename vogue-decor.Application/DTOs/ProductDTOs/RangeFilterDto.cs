namespace vogue_decor.Application.DTOs.ProductDTOs;

/// <summary>
/// DTO для диапазонных фильтров
/// </summary>
public class RangeFilterDto
{
    /// <summary>
    /// Минимальное значение
    /// </summary>
    public decimal MinValue { get; init; }
        
    /// <summary>
    /// Максимальное значение
    /// </summary>
    public decimal MaxValue { get; init; } 
}