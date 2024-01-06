using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.CollectionDTOs;

/// <summary>
/// DTO для получения коллекции по имени
/// </summary>
public class GetCollectionByNameDto
{
    /// <summary>
    /// Идентификатор коллекции
    /// </summary>
    [Required(ErrorMessage = "Идентификатор коллекции обязателен")]
    public string Name { get; set; } = null!;
}