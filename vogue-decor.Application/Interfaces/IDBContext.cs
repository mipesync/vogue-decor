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
        /// Получить/установить список типов люстр
        /// </summary>
        DbSet<ChandelierType> ChandelierTypes { get; set; }
        /// <summary>
        /// Получить/установаить список файлов
        /// </summary>
        DbSet<File> Files { get; set; }

        /// <summary>
        ///     Ассинхронно сохраняет сделанные изменения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
