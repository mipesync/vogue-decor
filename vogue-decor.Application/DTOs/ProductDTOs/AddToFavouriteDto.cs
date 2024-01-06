namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для добавления товара в избранные
    /// </summary>
    public class AddToFavouriteDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; } = Guid.Empty;
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid ProductId { get; set; } = Guid.Empty;
    }
}
