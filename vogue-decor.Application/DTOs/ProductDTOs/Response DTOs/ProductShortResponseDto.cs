namespace vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs
{
    public class ProductShortResponseDto
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Название товара
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Артикул товара
        /// </summary>
        public string Article { get; set; } = null!;
        /// <summary>
        /// Цена товара
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Скидка товара
        /// </summary>
        public int? Discount { get; set; }
        /// <summary>
        /// Флаг, показывающий, имеет ли товар скидку
        /// </summary>
        public bool IsSale { get; set; }
        /// <summary>
        /// Рейтинг товара
        /// </summary>
        public int Rating { get; set; }
        /// <summary>
        /// Наличие товара
        /// </summary>
        public int Availability { get; set; }
        /// <summary>
        /// Файлы товара (фото, видео)
        /// </summary>
        public string[]? Urls { get; set; }
        /// <summary>
        /// Находится ли товар в "Избранных"
        /// </summary>
        public bool IsFavourite { get; set; }
        /// <summary>
        /// Находится ли товар в корзине
        /// </summary>
        public bool IsCart { get; set; }
        /// <summary>
        /// Количество товара в корзине
        /// </summary>
        public int Quantity{ get; set; }
        /// <summary>
        /// Дата публикации товара
        /// </summary>
        public DateTime PublicationDate { get; set; }
        /// <summary>
        /// Количество покупок
        /// </summary>
        public int PurchasedCount { get; set; }
        /// <summary>
        /// Индекс товара
        /// </summary>
        public int Index { get; set; }
    }
}
