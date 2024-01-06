using vogue_decor.Application.Common.Services;
using vogue_decor.Domain;
using Microsoft.AspNetCore.Http;

namespace vogue_decor.Application.Interfaces
{
    /// <summary>
    /// Интерфейс парсера файлов со списком товаров
    /// </summary>
    public interface IFileParser
    {
        /// <summary>
        /// Преобразовать список товаров из файла в список <see cref="Product"/>
        /// </summary>
        /// <param name="file">Файл со списком товаров</param>
        /// <returns><see cref="ProductsParserDto"/></returns>
        Task<ProductsParserDto> ParseAsync(IFormFile file);
    }
}
