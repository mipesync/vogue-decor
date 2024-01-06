using vogue_decor.Application.Interfaces;
using vogue_decor.Domain;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;
using System.Xml.Serialization;
using vogue_decor.Application.DTOs.ProductDTOs;
using NpgsqlTypes;

namespace vogue_decor.Application.Common.Services
{
    /// <summary>
    /// Парсер файла со списком товаров
    /// </summary>
    /// <inheritdoc/>
    public class FileParser : IFileParser
    {
        public async Task<ProductsParserDto> ParseAsync(IFormFile file)
        {
            XDocument xml = new XDocument();

            using (MemoryStream str = new MemoryStream())
            {
                await file.CopyToAsync(str);
                str.Position = 0;
                xml = XDocument.Load(str);
            }

            var reader = new StringReader(xml.ToString());
            XmlSerializer l_serializer = new XmlSerializer(typeof(ProductsParserDto));
            var products = (ProductsParserDto)l_serializer.Deserialize(reader)!;
            
            return products;
        }
    }

    /// <summary>
    /// DTO для парсера их XML файла
    /// </summary>
    [Serializable]
    [XmlRoot("products")]
    public class ProductsParserDto
    {
        /// <summary>
        /// Список товаров
        /// </summary>
        [XmlElement("product")]
        public List<CreateProductDto> ProductList { get; set; } = new ();
    }   
}
