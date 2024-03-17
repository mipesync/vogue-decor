using vogue_decor.Domain.Interfaces.ProductFields;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для обновления информации о товаре
    /// </summary>
    public class UpdateProductDto : IDimensions, IDiameter, IIndent, IPictureMaterial
    {
        /// <summary>
        /// Идентификатор товара
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
        public int? Type { get; set; }
        
        /// <summary>
        /// Категории товара
        /// </summary>
        public int[]? Categories { get; set; }
        
        /// <summary>
        /// Артикул товара
        /// </summary>
        public string? Article { get; set; }
        
        /// <summary>
        /// Цена товара
        /// </summary>
        public decimal? Price { get; set; }
        
        /// <summary>
        /// Список цветов товара
        /// </summary>
        public int[]? Colors { get; set; }
        
        /// <summary>
        /// Скидка на товар
        /// </summary>
        /// <remarks>Указывается в процентах</remarks>
        public uint? Discount { get; set; }
        
        /// <summary>
        /// Стиль товара
        /// </summary>
        public int[]? Styles { get; set; }
        
        /// <summary>
        /// Материал товара
        /// </summary>
        public int[]? Materials { get; set; }
        
        /// <summary>
        /// Наличие товара
        /// </summary>
        public int? Availability { get; set; }
        
        /// <summary>
        /// Идентификатор бренда
        /// </summary>
        public Guid? BrandId { get; set; }
        
        /// <summary>
        /// Идентификатор колекции
        /// </summary>
        public Guid? CollectionId { get; set; }
        
        public decimal[]? Height { get; set; }
        
        public decimal[]? Length { get; set; }
        
        public decimal[]? Width { get; set; }
        
        public decimal? Diameter { get; set; }
        
        public int[]? PictureMaterial { get; set; }
        
        public decimal? Indent { get; set; }
        
        /// <summary>
        /// Количество лампочек
        /// </summary>
        public int? LampCount { get; set; }
        
        /// <summary>
        /// Цоколь лампочек
        /// </summary>
        public string? Plinth { get; set; }
        
        /// <summary>
        /// Список типов люстры
        /// </summary>
        public int[]? ChandelierTypes { get; set; }
    }
}
