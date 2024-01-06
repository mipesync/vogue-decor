using System.Xml.Serialization;
using vogue_decor.Domain.Enums;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO создания товара
    /// </summary>
    [Serializable]
    [XmlRoot("product")]
    public class CreateProductDto
    {
        /// <summary>
        /// Название товара
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Описание товара
        /// </summary>
        [XmlElement("description")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Тип товара
        /// </summary>
        [XmlElement("type")]
        public ProductTypes Type { get; set; } = ProductTypes.NONE;
        /// <summary>
        /// Артикль товара
        /// </summary>
        [XmlElement("article")]
        public string Article { get; set; } = string.Empty;
        /// <summary>
        /// Цена товара
        /// </summary>
        [XmlElement("price")]
        public decimal Price { get; set; } = 0m;
        /// <summary>
        /// Цвет товара
        /// </summary>
        [XmlElement("color")]
        public int[]? Colors { get; set; }
        /// <summary>
        /// Диаметр товара
        /// </summary>
        [XmlElement("diameter")]
        public int Diameter { get; set; } = 0;
        /// <summary>
        /// Высота товара
        /// </summary>
        [XmlElement("heigth")]
        public int Height { get; set; } = 0;
        /// <summary>
        /// Длина товара
        /// </summary>
        [XmlElement("length")]
        public int Length { get; set; } = 0;
        /// <summary>
        /// Ширина товара
        /// </summary>
        [XmlElement("width")]
        public int Width { get; set; } = 0;
        /// <summary>
        /// Скидка товара
        /// </summary>
        [XmlElement("discount")]
        public int Discount { get; set; } = 0;
        /// <summary>
        /// Тип люстры
        /// </summary>
        [XmlElement("chandelierTypes")]
        public int[]? ChandelierTypes { get; set; }
        /// <summary>
        /// Цоколь лампочки
        /// </summary>
        [XmlElement("plinth")]
        public string Plinth { get; set; } = string.Empty;
        /// <summary>
        /// Количество лампочек
        /// </summary>
        [XmlElement("lampCount")]
        public int LampCount { get; set; } = 0;
        /// <summary>
        /// Рейтинг товара
        /// </summary>
        [XmlElement("rating")]
        public int Rating { get; set; } = 0;
        /// <summary>
        /// Наличие товара
        /// </summary>
        [XmlElement("availability")]
        public int Availability { get; set; } = 0;
        /// <summary>
        /// Идентификатор коллекции
        /// </summary>
        [XmlElement("collectionId")]
        public Guid CollectionId { get; set; } = Guid.Empty;
        /// <summary>
        /// Ссылки на изображения
        /// </summary>
        [XmlElement("urls")]
        public List<string>? Urls { get; set; }
    }
}
