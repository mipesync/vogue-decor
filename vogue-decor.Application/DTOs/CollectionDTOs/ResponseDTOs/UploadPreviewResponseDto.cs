namespace vogue_decor.Application.DTOs.CollectionDTOs.ResponseDTOs
{
    /// <summary>
    /// DTO, возвращаемое из метода загрузки обложки колекции
    /// </summary>
    public class UploadPreviewResponseDto
    {
        /// <summary>
        /// Ссылка на загруженную обложку
        /// </summary>
        public string PreviewUrl { get; set; } = string.Empty;
    }
}
