namespace vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs
{
    /// <summary>
    /// DTO, возвращаемое из метода получения кол-ва товаров по критерию
    /// </summary>
    public class ProductsCountResponseDto
    {
        /// <summary>
        /// Количество товаров
        /// </summary>
        public int Count { get; set; } = 0;
    }
}
