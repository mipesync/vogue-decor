using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.CollectionDTOs
{
    /// <summary>
    /// DTO для добавления новой коллекции
    /// </summary>
    public class AddCollectionDto
    {
        /// <summary>
        /// Название коллекции
        /// </summary>
        [Required(ErrorMessage = "Название обязательно")]
        public string Name { get; set; } = string.Empty;
    }
}
