namespace vogue_decor.Application.DTOs.CollectionDTOs.ResponseDTOs
{
    /// <summary>
    /// DTO, возвращаемое из метода добавления коллекции
    /// </summary>
    public class AddCollectionResponseDto
    {
        /// <summary>
        /// Идентификатор созданной коллекции
        /// </summary>
        public Guid CollectionId { get; set; } = Guid.Empty;
    }
}
