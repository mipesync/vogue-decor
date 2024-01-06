using System.ComponentModel.DataAnnotations;
using vogue_decor.Domain.Enums;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для поиска альбома по запросу
    /// </summary>
    public class SearchProductDto
    {
        /// <summary>
        /// Строка запроса
        /// </summary>
        [Required(ErrorMessage = "Строка запроса обязательна")]
        public string SearchQuery { get; set; } = string.Empty;
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; } = Guid.Empty;
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
