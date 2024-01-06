using vogue_decor.Domain.Enums;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для обновления информации о товаре
    /// </summary>
    public class UpdateProductDto
    {
        /// <summary>
        /// Инетификатор товара
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// Название товара
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Описание товара
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Тип товара
        /// </summary>
        public ProductTypes? Type { get; set; }
        /// <summary>
        /// Артикль товара
        /// </summary>
        public string? Article { get; set; }
        /// <summary>
        /// Цена товара
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// Цвет товара
        /// </summary>
        public List<Colors>? Colors { get; set; }
        /// <summary>
        /// Диаметр товара
        /// </summary>
        public int? Diameter { get; set; }
        /// <summary>
        /// Высота товара
        /// </summary>
        public int? Height { get; set; }
        /// <summary>
        /// Длина товара
        /// </summary>
        public int? Length { get; set; }
        /// <summary>
        /// Ширина товара
        /// </summary>
        public int? Width { get; set; }
        /// <summary>
        /// Скидка товара
        /// </summary>
        public int? Discount { get; set; }
        /// <summary>
        /// Тип люстры
        /// </summary>
        public List<ChandelierTypes>? ChandelierTypes { get; set; }
        /// <summary>
        /// Цоколь лампочки
        /// </summary>
        public string? Plinth { get; set; }
        /// <summary>
        /// Количество лампочек
        /// </summary>
        public int? LampCount { get; set; }
        /// <summary>
        /// Рейтинг товара
        /// </summary>
        public int? Rating { get; set; }
        /// <summary>
        /// Наличие товара
        /// </summary>
        public int? Availability { get; set; }
        /// <summary>
        /// Идентификатор коллекции
        /// </summary>
        public Guid? CollectionId { get; set; }
    }
}
