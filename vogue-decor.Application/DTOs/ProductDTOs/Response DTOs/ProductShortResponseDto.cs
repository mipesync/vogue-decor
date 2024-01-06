namespace vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs
{
    public class ProductShortResponseDto
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;
        /// <summary>
        /// Название товара
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Артикул товара
        /// </summary>
        public string Article { get; set; } = string.Empty;
        /// <summary>
        /// Цена товара
        /// </summary>
        public decimal Price { get; set; } = 0m;
        /// <summary>
        /// Скидка товара
        /// </summary>
        public int Discount { get; set; } = 0;
        /// <summary>
        /// Рейтинг товара
        /// </summary>
        public int Rating { get; set; } = 0;
        /// <summary>
        /// Наличие товара
        /// </summary>
        public int Availability { get; set; } = 0;
        /// <summary>
        /// Файлы товара (фото, видео)
        /// </summary>
        public string[]? Urls { get; set; }
        /// <summary>
        /// Находится ли товар в "Избранных"
        /// </summary>
        public bool IsFavourite { get; set; } = false;
        /// <summary>
        /// Находится ли товар в корзине
        /// </summary>
        public bool IsCart { get; set; } = false;
        /// <summary>
        /// Количество товара в корзине
        /// </summary>
        public int Quantity{ get; set; } = 0;
        /// <summary>
        /// Дата публикации товара
        /// </summary>
        public DateTime PublicationDate { get; set; }
        /// <summary>
        /// Количество покупок
        /// </summary>
        public int PurchasedCount { get; set; }
    }
}
