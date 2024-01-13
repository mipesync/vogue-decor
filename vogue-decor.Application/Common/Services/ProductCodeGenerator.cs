using vogue_decor.Application.Interfaces;

namespace vogue_decor.Application.Common.Services;

/// <summary>
/// Генератор кода товара
/// </summary>
public class ProductCodeGenerator : IProductCodeGenerator
{
    public string GenerateCode(string name, string brand, string publicationDate, string type, string article, string color,
        int length = 8)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(brand) || string.IsNullOrEmpty(article) || string.IsNullOrEmpty(color))
        {
            throw new ArgumentException("Все параметры должны быть непустыми строками");
        }
        if (length is < 6 or > 10)
        {
            throw new ArgumentException("Длина кода товара должна быть от 6 до 10 цифр");
        }

        var input = $"{name},{brand},{publicationDate},{type},{article},{color}";

        var hash = input.GetHashCode();

        var code = Math.Abs(hash).ToString();

        if (code.Length > length)
        {
            return code[..length];
        }

        return code.Length < length ? code.PadLeft(length, '0') : code;
    }
}