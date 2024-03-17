using System.Xml.Serialization;
using NpgsqlTypes;
using vogue_decor.Domain.Interfaces.ProductFields;

namespace vogue_decor.Domain
{
    /// <summary>
    /// Класс товара
    /// </summary>
    [Serializable]
    [XmlRoot("product")]
    public class Product : IBaseProduct, IDimensions, IDiameter, IPictureMaterial, IIndent
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid Id { get; set; }
        
        [XmlElement("name")]
        public string Name { get; set; } = null!;
        
        [XmlElement("description")]
        public string? Description { get; set; }
        /// <summary>
        /// Тип товара
        /// </summary>
        [XmlElement("productType")]
        public int ProductType { get; set; }
        /// <summary>
        /// Тип конкретного товара
        /// </summary>
        [XmlElement("categories")]
        public int[] Types { get; set; } = null!;
        
        [XmlElement("article")]
        public string Article { get; set; } = null!;
        
        public string Code { get; set; } = null!;
        
        [XmlElement("price")]
        public decimal Price { get; set; }
        
        [XmlElement("color")]
        public int[] Colors { get; set; } = null!;
        
        [XmlElement("diameter")]
        public decimal? Diameter { get; set; }
        
        [XmlElement("height")]
        public decimal[]? Height { get; set; }
        
        [XmlElement("length")]
        public decimal[]? Length { get; set; }
        
        [XmlElement("width")]
        public decimal[]? Width { get; set; }
        
        [XmlElement("pictureMaterial")]
        public int[]? PictureMaterial { get; set; }
        
        [XmlElement("indent")]
        public decimal? Indent { get; set; }

        [XmlElement("urls")]
        public List<string> Urls { get; set; } = new();
        
        [XmlElement("discount")]
        public uint? Discount { get; set; }
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
        /// Типы люстр
        /// </summary>
        [XmlElement("chandelierTypes")]
        public int[]? ChandelierTypes { get; set; }
        /// <summary>
        /// Рейтинг товара
        /// </summary>
        public decimal? Rating { get; set; }
        /// <summary>
        /// Наличие товара
        /// </summary>
        public int Availability { get; set; }
        /// <summary>
        /// Идентификатор коллекции
        /// </summary>
        public Guid? CollectionId { get; set; }
        /// <summary>
        /// Идентификатор бренда
        /// </summary>
        public Guid BrandId { get; set; }
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
        /// Список стилей товара
        /// </summary>
        public int[]? Styles { get; set; }
        /// <summary>
        /// Список материалов товара
        /// </summary>
        public int[]? Materials { get; set; }
        /// <summary>
        /// Индекс товара
        /// </summary>
        public int? Index { get; set; }
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
        /// Бренд товара
        /// </summary>
        public Brand? Brand { get; set; }
        /// <summary>
        /// Список промежуточных сущностей "Избранные"
        /// </summary>
        public List<Favourite> Favourites { get; set; } = new();
        /// <summary>
        /// Список объектов стилей
        /// </summary>
        public List<Style> StylesObj { get; set; } = new();
        public List<ProductStyle> ProductStyles { get; set; } = new();
        /// <summary>
        /// Список объектов материалов
        /// </summary>
        public List<Material> MaterialsObj { get; set; } = new();
        public List<ProductMaterial> ProductMaterials { get; set; } = new();
    }
}
