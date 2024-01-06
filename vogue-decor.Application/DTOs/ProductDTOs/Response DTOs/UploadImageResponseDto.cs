namespace vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs
{
    /// <summary>
    /// DTO, возвращаемое из метода загрузки изображения
    /// </summary>
    public class UploadImageResponseDto
    {
        public List<string> Files { get; set;} = new List<string>();
    }
}
