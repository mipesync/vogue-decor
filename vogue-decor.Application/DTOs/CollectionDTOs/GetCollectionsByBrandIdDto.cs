using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.CollectionDTOs;

/// <summary>
/// DTO для получения коллекций бренда
/// </summary>
public class GetCollectionsByBrandIdDto
{
    /// <summary>
    /// Идентификатор бренда
    /// </summary>
    [Required(ErrorMessage = "Идентификатор бренда обязателен")]
    public Guid BrandId { get; set; }
    
    /// <summary>
    /// С какого количества элементов начинать выборку
    /// </summary>
    public int From { get; set; } = 0;

    /// <summary>
    /// Сколько элементов необходимо вывести
    /// </summary>
    public int Count { get; set; } = 10;
}