using vogue_decor.Domain.Enums;
using NpgsqlTypes;
using System.Xml.Serialization;

namespace vogue_decor.Domain
{
    /// <summary>
    /// Класс товара
    /// </summary>
    [Serializable]
    [XmlRoot("product")]
    public class Product
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;
        /// <summary>
        /// Название товара
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Описание товара
        /// </summary>
        [XmlElement("description")]
        public string? Description { get; set; }
        /// <summary>
        /// Тип товара
        /// </summary>
        [XmlElement("type")]
        public int Type { get; set; }
        /// <summary>
        /// Артикул товара
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
        public int? Diameter { get; set; }
        /// <summary>
        /// Высота товара
        /// </summary>
        [XmlElement("heigth")]
        public int? Height { get; set; }
        /// <summary>
        /// Длина товара
        /// </summary>
        [XmlElement("length")]
        public int? Length { get; set; }
        /// <summary>
        /// Ширина товара
        /// </summary>
        [XmlElement("width")]
        public int? Width { get; set; }
        /// <summary>
        /// Файлы товара (фото, видео)
        /// </summary>
        [XmlElement("urls")]
        public List<string> Urls { get; set; } = new();
        /// <summary>
        /// Скидка товара
        /// </summary>
        public int? Discount { get; set; }

        /// <summary>
        /// Тип люстры
        /// </summary>
        public int[]? ChandelierTypes { get; set; }
        /// <summary>
        /// Цоколь лампочки
        /// </summary>
        [XmlElement("plinth")]
        public string? Plinth { get; set; }
        /// <summary>
        /// Количество лампочек
        /// </summary>
        [XmlElement("lampCount")]
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
        /// <summary>
        /// Дата публикации товара
        /// </summary>
        public DateTime PublicationDate { get; set; }
        /// <summary>
        /// Количество покупок
        /// </summary>
        public int? PurchasedCount { get; set; }
        /// <summary>
        /// Вектор поиска
        /// </summary>
        [XmlIgnore]
        public virtual NpgsqlTsVector? SearchVector { get; set; }

        /// <summary>
        /// Список пользователей, которые добавили товар в корзину
        /// </summary>
        public List<User> Users { get; set; } = new();
        /// <summary>
        /// Список промежуточных сущностей "Корзина"
        /// </summary>
        public List<ProductUser> ProductUsers { get; set; } = new();
        /// <summary>
        /// Коллекция товара
        /// </summary>
        public Collection? Collection { get; set; }
        /// <summary>
        /// Список промежуточных сущностей "Избранные"
        /// </summary>
        public List<Favourite> Favourites { get; set; } = new();
    }
}
