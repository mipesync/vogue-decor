namespace vogue_decor.Application.DTOs.CollectionDTOs
{
    /// <summary>
    /// DTO для установки индекса порядка коллекции
    /// </summary>
    public class SetCollectionIndexDto
    {
        /// <summary>
        /// Идентификатор коллекции
        /// </summary>
        public Guid CollectionId { get; set; }
        /// <summary>
        /// Индекс порядка
        /// </summary>
        public int Index { get; set; }
    }
}
