namespace vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs
{
    /// <summary>
    /// DTO, возвращаемое из метода получения списков товаров
    /// </summary>
    public class GetProductsResponseDto
    {
        /// <summary>
        /// Список товаров
        /// </summary>
        public List<ProductShortResponseDto> Products { get; set; } = new();
        /// <summary>
        /// Количество товаров 
        /// </summary>
        public int TotalCount { get; set; } = 0;
        /// <summary>
        /// Количество товаров в корзине
        /// </summary>
        public int CartCount { get; set; } = 0;
        /// <summary>
        /// Количество товаров в избранном
        /// </summary>
        public int FavouritesCount { get; set; } = 0;
        /// <summary>
        /// Название бренда
        /// </summary>
        public string? BrandName { get; set; }
    }
}
