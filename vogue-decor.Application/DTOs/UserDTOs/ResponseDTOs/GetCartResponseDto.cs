namespace vogue_decor.Application.DTOs.UserDTOs.ResponseDTOs
{
    /// <summary>
    /// DTO, возвращаемое из метода получения корзины
    /// </summary>
    public class GetCartResponseDto
    {
        /// <summary>
        /// Товары, находящиеся в корзине
        /// </summary>
        public List<CartResponseDto> Products { get; set; } = new List<CartResponseDto>();
        /// <summary>
        /// Количество товаров в корзине
        /// </summary>
        public int CartCount { get; set; } = 0;
        /// <summary>
        /// Количество товаров в избранном
        /// </summary>
        public int FavouritesCount { get; set; } = 0;
    }
}
