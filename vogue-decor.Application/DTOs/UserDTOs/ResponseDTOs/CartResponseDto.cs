using vogue_decor.Application.DTOs.BrandDTOs;
using vogue_decor.Application.DTOs.CollectionDTOs;
using vogue_decor.Application.DTOs.ProductDTOs;
using vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs;
using vogue_decor.Domain;

namespace vogue_decor.Application.DTOs.UserDTOs.ResponseDTOs;

public class CartResponseDto : ProductResponseDto
{        
    /// <summary>
    /// Идентификатор товара
    /// </summary>
    public Guid Id { get; set; } = Guid.Empty;
    /// <summary>
    /// Название товара
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Описание товара
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Тип товара
    /// </summary>
    public int Type { get; set; } = new();
    /// <summary>
    /// Артикль товара
    /// </summary>
    public string Article { get; set; } = string.Empty;
    /// <summary>
    /// Цена товара
    /// </summary>
    public decimal Price { get; set; } = 0m;
    /// <summary>
    /// Цвет товара
    /// </summary>
    public int[]? Colors { get; set; }
    /// <summary>
    /// Диаметр товара
    /// </summary>
    public decimal Diameter { get; set; } = 0m;
    /// <summary>
    /// Высота товара
    /// </summary>
    public decimal[]? Height { get; set; }
    /// <summary>
    /// Длина товара
    /// </summary>
    public decimal[]? Length { get; set; }
    /// <summary>
    /// Ширина товара
    /// </summary>
    public decimal[]? Width { get; set; }
    /// <summary>
    /// Скидка товара
    /// </summary>
    public int Discount { get; set; } = 0;
    /// <summary>
    /// Тип люстры
    /// </summary>
    public int[]? ChandelierTypes { get; set; }
    /// <summary>
    /// Цоколь лампочки
    /// </summary>
    public string Plinth { get; set; } = string.Empty;
    /// <summary>
    /// Количество лампочек
    /// </summary>
    public int LampCount { get; set; } = 0;
    /// <summary>
    /// Рейтинг товара
    /// </summary>
    public int Rating { get; set; } = 0;
    /// <summary>
    /// Наличие товара
    /// </summary>
    public int Availability { get; set; } = 0;
    /// <summary>
    /// Файлы товара (фото, видео)
    /// </summary>
    public List<FileDto> Files { get; set; } = new();
    /// <summary>
    /// Находится ли товар в "Избранных"
    /// </summary>
    public bool IsFavourite { get; set; } = false;
    /// <summary>
    /// Находится ли товар в корзине
    /// </summary>
    public bool IsCart { get; set; } = false;
    /// <summary>
    /// Количество товара в корзине
    /// </summary>
    public int Quantity { get; set; } = 0;
    /// <summary>
    /// Дата публикации товара
    /// </summary>
    public DateTime PublicationDate { get; set; }
    /// <summary>
    /// Количество покупок
    /// </summary>
    public int PurchasedCount { get; set; }
    /// <summary>
    /// Количество товара
    /// </summary>
    public int Count { get; set; } = 0;
    /// <summary>
    /// Коллекция товара
    /// </summary>
    public ShortCollectionDto? Collection { get; set; }
    /// <summary>
    /// Бренд товара
    /// </summary>
    public ShortBrandDto? Brand { get; set; }
}