namespace vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs
{
    /// <summary>
    /// DTO, возвращаемое из метода создания (добавления) товара
    /// </summary>
    public class CreateProductResponseDto
    {
        /// <summary>
        /// Идентификатор добавленного товара
        /// </summary>
        public Guid ProductId { get; set; } = Guid.Empty;
    }
}
