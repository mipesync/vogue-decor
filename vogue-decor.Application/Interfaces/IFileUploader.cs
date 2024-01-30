using Microsoft.AspNetCore.Http;

namespace vogue_decor.Application.Interfaces
{
    /// <summary>
    /// Интерфес загрузчика файлов
    /// </summary>
    public interface IFileUploader
    {
        /// <summary>
        /// Загружаемый файл
        /// </summary>
        IFormFile? File { get; set; }
        /// <summary>
        /// Ссылка на загружаемый файл
        /// </summary>
        string? FileUrl { get; set; }
        /// <summary>
        /// Абсолютный путь к файлу
        /// </summary>
        string AbsolutePath { get; set; }
        /// <summary>
        /// Корневой путь проекта
        /// </summary>
        string WebRootPath { get; set; }

        /// <summary>
        /// Загрузить файл асинхронно
        /// </summary>
        /// <param name="mutable">Изменять ли размер изображения</param>
        /// <returns>Название загруженного файла</returns>
        Task<string[]> UploadFileAsync(bool mutable = true);
    }
}
