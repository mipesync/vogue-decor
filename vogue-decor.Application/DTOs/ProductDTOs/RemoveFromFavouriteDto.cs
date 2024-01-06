﻿namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для удаления товара из избранных
    /// </summary>
    public class RemoveFromFavouriteDto
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
