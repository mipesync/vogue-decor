using System.Text.Json.Serialization;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для удаления товара из корзины
    /// </summary>
    public class RemoveFromCartDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonIgnore]
        public Guid UserId { get; set; } = Guid.Empty;
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid ProductId { get; set; } = Guid.Empty;
        /// <summary>
        /// Удалить весь товар или одну единицу?
        /// </summary>
        public bool IsRemovingAll { get; set; } = false;
    }
}
