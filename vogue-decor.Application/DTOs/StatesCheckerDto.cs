﻿namespace vogue_decor.Application.DTOs
{
    /// <summary>
    /// DTO, возвращаемое из проверки состояний товара
    /// </summary>
    public class StatesCheckerDto
    {
        /// <summary>
        /// Добавлен ли в избранные
        /// </summary>
        public bool IsFavourite { get; set; } = false;
        /// <summary>
        /// Добавлен ли в корзину
        /// </summary>
        public bool IsCart { get; set; } = false;
    }
}
