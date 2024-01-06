using vogue_decor.Domain.Enums;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для получения количества товаров по критериям
    /// </summary>
    public class GetProductsCountDto
    {
        /// <summary>
        /// Типы товаров
        /// </summary>
        public int[]? Types { get; set; }
        /// <summary>
        /// Минимальная цена товара
        /// </summary>
        public decimal MinPrice { get; set; } = 0m;
        /// <summary>
        /// Максимальная цена товара
        /// </summary>
        public decimal MaxPrice { get; set; } = decimal.MaxValue;
        /// <summary>
        /// Цвет товара
        /// </summary>
        public int[]? Colors { get; set; }
        /// <summary>
        /// Типы люстры
        /// </summary>
        public int[]? ChandelierTypes { get; set; }
        /// <summary>
        /// Минимальный диаметр товара
        /// </summary>
        public int MinDiameter { get; set; } = 0;
        /// <summary>
        /// Максимальный диаметр товара
        /// </summary>
        public int MaxDiameter { get; set; } = int.MaxValue;
        /// <summary>
        /// Минимальное количество лампочек
        /// </summary>
        public int MinLampCount { get; set; } = 0;
        /// <summary>
        /// Максимальное количество лампочек
        /// </summary>
        public int MaxLampCount { get; set; } = int.MaxValue;
        /// <summary>
        /// Идентификатор коллекции
        /// </summary>
        public Guid? CollectionId { get; set; }
        /// <summary>
        /// Участие товаров в распродаже
        /// </summary>
        public bool IsSale { get; set; } = false;
    }
}
