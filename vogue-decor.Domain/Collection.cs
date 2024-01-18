using vogue_decor.Domain.Interfaces;

namespace vogue_decor.Domain
{
    /// <summary>
    /// Класс коллекции
    /// </summary>
    public class Collection : ISecondaryEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
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
