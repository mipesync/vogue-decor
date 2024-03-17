namespace vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs
{
    public class ProductResponseDto
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
        /// Описание товара
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Тип товара
        /// </summary>
        public int Type { get; set; } = new();
        /// <summary>
        /// Артикль товара
        /// </summary>
        public string Article { get; set; } = string.Empty;
        /// <summary>
        /// Цена товара
        /// </summary>
        public decimal Price { get; set; } = 0m;
        /// <summary>
        /// Цвет товара
        /// </summary>
        public int[]? Colors { get; set; }
        /// <summary>
        /// Диаметр товара
        /// </summary>
        public decimal Diameter { get; set; } = 0;
        /// <summary>
        /// Высота товара
        /// </summary>
        public decimal[]? Height { get; set; }

        /// <summary>
        /// Длина товара
        /// </summary>
        public decimal[]? Length { get; set; }

        /// <summary>
        /// Ширина товара
        /// </summary>
        public decimal[]? Width { get; set; }
        /// <summary>
        /// Скидка товара
        /// </summary>
        public int Discount { get; set; } = 0;
        /// <summary>
        /// Тип люстры
        /// </summary>
        public int[]? ChandelierTypes { get; set; }
        /// <summary>
        /// Цоколь лампочки
        /// </summary>
        public string Plinth { get; set; } = string.Empty;
        /// <summary>
        /// Количество лампочек
        /// </summary>
        public int LampCount { get; set; } = 0;
        /// <summary>
        /// Рейтинг товара
        /// </summary>
        public int Rating { get; set; } = 0;
        /// <summary>
        /// Наличие товара
        /// </summary>
        public int Availability { get; set; } = 0;
        /// <summary>
        /// Идентификатор коллекции
        /// </summary>
        public Guid? CollectionId { get; set; }
        /// <summary>
        /// Файлы товара (фото, видео)
        /// </summary>
        public List<FileDto> Files { get; set; } = new();
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
        public int Quantity { get; set; } = 0;
        /// <summary>
        /// Количество товаров в корзине
        /// </summary>
        public int CartCount { get; set; } = 0;
        /// <summary>
        /// Количество товаров в избранном
        /// </summary>
        public int FavouritesCount { get; set; } = 0;
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
        /// <summary>
        /// Код товара
        /// </summary>
        public long Code { get; set; }
    }

    /// <summary>
    /// DTO для отображения url к файлу вместе с названием
    /// </summary>
    public class FileDto
    {
        /// <summary>
        /// Название файла
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Статическая ссылка на файл
        /// </summary>
        public string Url { get; set; } = string.Empty;
    }
}
