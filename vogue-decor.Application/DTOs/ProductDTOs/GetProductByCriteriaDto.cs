using vogue_decor.Domain.Enums;

namespace vogue_decor.Application.DTOs.ProductDTOs
{
    /// <summary>
    /// DTO для отбора товаров по критериям
    /// </summary>
    public class GetProductByCriteriaDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; } = Guid.Empty;
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
        /// С какого количества начать выборку товаров (по умолчанию - 0)
        /// </summary>
        public int From { get; set; } = 0;
        /// <summary>
        /// Количество товара, которое надо получить (по умолчанию - 10)
        /// </summary>
        public int Count { get; set; } = 10;
        /// <summary>
        /// Участие товаров в распродаже
        /// </summary>
        public bool? IsSale { get; set; }
        /// <summary>
        /// Тип сортировки
        /// </summary>
        public SortTypes? SortType { get; set; }
        /// <summary>
        /// Идентификатор коллекции
        /// </summary>
        public Guid? CollectionId { get; set; }
    }
}
