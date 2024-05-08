using System.Text.Json.Serialization;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для получения списка всех товаров постранично
    /// </summary>
    public class GetAllProductsDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonIgnore]
        public Guid UserId { get; set; } = Guid.Empty;
        /// <summary>
        /// С какого количества начать выборку товаров (по умолчанию - 0)
        /// </summary>
        public int From { get; set; } = 0;
        /// <summary>
        /// Количество товара, которое надо получить (по умолчанию - 10)
        /// </summary>
        public int Count{ get; set; } = 10;
    }
}
