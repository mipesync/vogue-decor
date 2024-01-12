namespace vogue_decor.Domain
{
    /// <summary>
    /// Класс коллекции
    /// </summary>
    public class Collection
    {
        /// <summary>
        /// Идентификатор коллекции
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;
        /// <summary>
        /// Название коллекции
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Обложка коллекции
        /// </summary>
        public string Preview { get; set; } = string.Empty;
        /// <summary>
        /// Порядок коллекции
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// Список товаров из коллекции
        /// </summary>
        public List<Product> Products { get; set; } = new();
    }
}
