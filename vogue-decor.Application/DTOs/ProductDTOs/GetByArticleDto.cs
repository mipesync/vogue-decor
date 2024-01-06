using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для получения товара по артикулу
    /// </summary>
    public class GetByArticleDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; } = Guid.Empty;
        /// <summary>
        /// Артикул товара
        /// </summary>
        [Required(ErrorMessage = "Артикул является обязательным")]
        public string Article { get; set; } = string.Empty;
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
