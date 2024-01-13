namespace vogue_decor.Application.Interfaces;

/// <summary>
/// Интрейфейс для генератора кода
/// </summary>
public interface IProductCodeGenerator
{
    /// <summary>
    /// Сгенерировать код товара
    /// </summary>
    /// <param name="name">Название товара</param>
    /// <param name="brand">Название бренда</param>
    /// <param name="publicationDate">Дата публикации товара</param>
    /// <param name="type">Тип товара</param>
    /// <param name="article">Артикул товара</param>
    /// <param name="color">Цвет товара</param>
    /// <param name="length">Длина генерируемого кода</param>
    /// <returns>Код товара</returns>
    public string GenerateCode(string name, string brand, string publicationDate, string type, string article, string color, int length = 8);
}