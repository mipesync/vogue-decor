using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using vogue_decor.Application.Common.Attributes;
using vogue_decor.Domain.Interfaces.ProductFields;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO создания товара
    /// </summary>
    [Serializable]
    [XmlRoot("product")]
    public class CreateProductDto : IDimensions, IDiameter, IPictureMaterial, IIndent
    {
        /// <summary>
        /// Название товара
        /// </summary>
        [XmlElement("name")]
        [Required(ErrorMessage = "Название товара обязательно")]
        public string Name { get; set; } = null!;
        
        /// <summary>
        /// Описание товара
        /// </summary>
        [XmlElement("description")]
        public string? Description { get; set; }
        
        /// <summary>
        /// Тип товара
        /// </summary>
        [XmlElement("productType")]
        [Required(ErrorMessage = "Тип товара обязателен")]
        public int Type { get; set; }
        
        /// <summary>
        /// Категории товара
        /// </summary>
        [XmlElement("categories")]
        [Required(ErrorMessage = "Категория(-и) товара обязательна(-ы)")]
        public int[] Categories { get; set; } = null!;
        
        /// <summary>
        /// Артикул товара
        /// </summary>
        [XmlElement("article")]
        [Required(ErrorMessage = "Артикул товара обязателен")]
        public string Article { get; set; } = null!;
        
        /// <summary>
        /// Цена товара
        /// </summary>
        [XmlElement("price")]
        [Required(ErrorMessage = "Цена товара обязательна")]
        public decimal Price { get; set; }
        
        /// <summary>
        /// Список цветов товара
        /// </summary>
        [XmlElement("color")]
        [Required(ErrorMessage = "Цвет(-а) обязателен(-ы)")]
        public int[] Colors { get; set; } = null!;
        
        /// <summary>
        /// Скидка на товар
        /// </summary>
        /// <remarks>Указывается в процентах</remarks>
        [XmlElement("discount")]
        public uint? Discount { get; set; }
        
        /// <summary>
        /// Стиль товара
        /// </summary>
        [XmlElement("style")]
        public int[]? Styles { get; set; }
        
        /// <summary>
        /// Материал товара
        /// </summary>
        [XmlElement("material")]
        public int[]? Materials { get; set; }
        
        /// <summary>
        /// Наличие товара
        /// </summary>
        [XmlElement("availability")]
        [Required(ErrorMessage = "Наличие товара обязательно")]
        public int Availability { get; set; }
        
        /// <summary>
        /// Идентификатор бренда
        /// </summary>
        [XmlElement("brandId")]
        [Required(ErrorMessage = "Идентификатор бренда обязателен")]
        public Guid BrandId { get; set; }
        
        /// <summary>
        /// Идентификатор колекции
        /// </summary>
        [XmlElement("collectionId")]
        public Guid? CollectionId { get; set; }

        /// <summary>
        /// Список ссылок на изображения
        /// </summary>
        [XmlElement("url")]
        [Required(ErrorMessage = "Ссылки на фото товара обязательны")]
        public List<string> Urls { get; set; } = null!;
        
        /// <summary>
        /// Список файлов на изображения
        /// </summary>
        [XmlIgnore]
        [ExtensionValidator(Extensions = "jpg,jpeg,png")]
        public List<IFormFile>? Files { get; set; }
        
        [XmlElement("height")]
        public decimal[]? Height { get; set; }
        
        [XmlElement("length")]
        public decimal[]? Length { get; set; }
        
        [XmlElement("width")]
        public decimal[]? Width { get; set; }
        
        [XmlElement("diameter")]
        public decimal? Diameter { get; set; }
        
        [XmlElement("pictureMaterial")]
        public int[]? PictureMaterial { get; set; }
        
        [XmlElement("indent")]
        public decimal? Indent { get; set; }
        
        /// <summary>
        /// Количество лампочек
        /// </summary>
        [XmlElement("lampCount")]
        public int? LampCount { get; set; }
        
        /// <summary>
        /// Цоколь лампочек
        /// </summary>
        [XmlElement("plinth")]
        public string? Plinth { get; set; }
        
        /// <summary>
        /// Список типов люстры
        /// </summary>
        [XmlElement("chandelierType")]
        public int[]? ChandelierTypes { get; set; }
    }
}
