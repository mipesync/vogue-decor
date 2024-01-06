namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для упорядочивания файлов товара
    /// </summary>
    public class SetFileOrderDto
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// Упорядоченные названия файлов
        /// </summary>
        public List<string> FileNames { get; set; } = new List<string>();
    }
}
