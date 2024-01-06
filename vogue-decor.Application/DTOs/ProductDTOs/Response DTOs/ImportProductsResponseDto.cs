namespace vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs
{
    /// <summary>
    /// DTO, возвращаемое из метода импорта списка товаров
    /// </summary>
    public class ImportProductsResponseDto
    {
        /// <summary>
        /// Список идентификаторов импортированных товаров
        /// </summary>
        public List<Guid> ProductIds { get; set; } = new List<Guid>();
    }
}
