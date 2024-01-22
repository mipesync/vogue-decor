namespace vogue_decor.Application.DTOs.ProductDTOs;

/// <summary>
/// DTO для обновления рейтинга товара
/// </summary>
public class UpdateRatingDto
{
    /// <summary>
    /// Идентификатор товара
    /// </summary>
    public Guid ProductId { get; set; }
    /// <summary>
    /// Оценка товара
    /// </summary>
    public int Estimation { get; set; }
}