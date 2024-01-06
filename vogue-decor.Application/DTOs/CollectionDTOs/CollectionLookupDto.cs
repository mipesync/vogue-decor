namespace vogue_decor.Application.DTOs.CollectionDTOs
{
    public class CollectionLookupDto
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
        /// Ссылка на обложку
        /// </summary>
        public string? Preview { get; set; } = string.Empty;
        /// <summary>
        /// Индекс порядка коллекции
        /// </summary>
        public int? Index { get; set; }
    }
}
