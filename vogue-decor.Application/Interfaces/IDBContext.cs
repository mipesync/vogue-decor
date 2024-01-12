using vogue_decor.Domain;
using vogue_decor.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using File = vogue_decor.Domain.File;

namespace vogue_decor.Application.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий таблицы, используемые в проекте
    /// </summary>
    public interface IDBContext
    {
        /// <summary>
        /// Получить/установить список пользователей
        /// </summary>
        DbSet<User> Users { get; set; }
        /// <summary>
        /// Получить/установить список товаров
        /// </summary>
        DbSet<Product> Products { get; set; }
        /// <summary>
        /// Получить/установить список промежуточных записей товара-пользователя
        /// </summary>
        DbSet<ProductUser> ProductUsers { get; set; }
        /// <summary>
        /// Получить/установить список логов
        /// </summary>
        DbSet<Log> Logs { get; set; }
        /// <summary>
        /// Получить/установить список избранного
        /// </summary>
        DbSet<Favourite> Favourites { get; set; }
        /// <summary>
        /// Получить/установить список коллекций
        /// </summary>
        DbSet<Collection> Collections { get; set; }
        /// <summary>
        /// Получить/установить список цветов
        /// </summary>
        DbSet<Color> Colors { get; set; }
        /// <summary>
        /// Получить/установить список типов товара
        /// </summary>
        DbSet<ProductType> ProductTypes { get; set; }
        /// <summary>
        /// Получить/установаить список файлов
        /// </summary>
        DbSet<File> Files { get; set; }
        /// <summary>
        /// Получить/установаить список брендов
        /// </summary>
        DbSet<Brand> Brands { get; set; }
        /// <summary>
        /// Получить/установаить список категорий
        /// </summary>
        DbSet<Category> Categories { get; set; }
        /// <summary>
        /// Получить/установаить список материалов
        /// </summary>
        DbSet<Material> Materials { get; set; }
        /// <summary>
        /// Получить/установаить список промежуточных записей товара-материалов
        /// </summary>
        DbSet<ProductMaterial> ProductMaterials { get; set; }
        /// <summary>
        /// Получить/установаить список промежуточных записей товара-стилей
        /// </summary>
        DbSet<ProductStyle> ProductStyles { get; set; }
        /// <summary>
        /// Получить/установаить список стилей
        /// </summary>
        DbSet<Style> Styles { get; set; }

        /// <summary>
        ///     Ассинхронно сохраняет сделанные изменения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
