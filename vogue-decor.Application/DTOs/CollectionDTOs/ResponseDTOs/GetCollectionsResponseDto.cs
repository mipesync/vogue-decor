namespace vogue_decor.Application.DTOs.CollectionDTOs.ResponseDTOs
{
    /// <summary>
    /// DTO, возвращаемое из метода получения всех коллекций
    /// </summary>
    public class GetCollectionsResponseDto
    {
        /// <summary>
        /// Список коллекций
        /// </summary>
        public List<CollectionLookupDto> Collections { get; set; } = new();
        /// <summary>
        /// Название бренда
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// Общее количество коллекций
        /// </summary>
        public int TotalCount { get; set; }
    }
}