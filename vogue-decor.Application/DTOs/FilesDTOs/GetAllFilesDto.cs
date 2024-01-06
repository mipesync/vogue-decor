namespace vogue_decor.Application.DTOs.FilesDTOs;

/// <summary>
/// DTO для получения списка файлов
/// </summary>
public class GetAllFilesDto
{
    /// <summary>
    /// С какого количества начать выборку файлов (по умолчанию - 0)
    /// </summary>
    public int From { get; set; } = 0;
    /// <summary>
    /// Количество файлов, которое надо получить (по умолчанию - 10)
    /// </summary>
    public int Count{ get; set; } = 10;
}