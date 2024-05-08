using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using vogue_decor.Domain.Enums;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для получения списка товаров по идентификатору коллекции
    /// </summary>
    public class GetProductByCollectionIdDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonIgnore]
        public Guid UserId { get; set; } = Guid.Empty;
        /// <summary>
        /// Идентификатор коллекции
        /// </summary>
        [Required(ErrorMessage = "Идентификатор коллекции обязателен")]
        public Guid CollectionId { get; set; } = Guid.Empty;
        /// <summary>
        /// С какого количества начать выборку товаров (по умолчанию - 0)
        /// </summary>
        public int From { get; set; } = 0;
        /// <summary>
        /// Количество товара, которое надо получить (по умолчанию - 10)
        /// </summary>
        public int Count { get; set; } = 10;
        /// <summary>
        /// Тип сортировки
        /// </summary>
        public SortTypes? SortType { get; set; }
    }
}
