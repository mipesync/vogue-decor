using vogue_decor.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace vogue_decor.Domain
{
    /// <summary>
    /// Класс пользователя
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Роль пользователя
        /// </summary>
        public Roles Role { get; set; } = Roles.NONE;
        /// <summary>
        /// Код подтверждения
        /// </summary>
        public string ConfirmationCode { get; set; } = string.Empty;

        /// <summary>
        /// Продукты пользователя
        /// </summary>
        public List<Product> Products { get; set; } = new List<Product>();
        /// <summary>
        /// Список промежуточных сущностей "Корзина"
        /// </summary>
        public List<ProductUser> ProductUsers { get; set; } = new List<ProductUser>();
        /// <summary>
        /// Логи пользователя
        /// </summary>
        public List<Log> Logs { get; set; } = new List<Log>();
        /// <summary>
        /// Список промежуточных сущностей "Избранные"
        /// </summary>
        public List<Favourite> Favourites { get; set; } = new List<Favourite>();
    }
}
