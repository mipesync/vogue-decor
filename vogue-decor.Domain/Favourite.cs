namespace vogue_decor.Domain
{
    /// <summary>
    /// Класс избранного
    /// </summary>
    public class Favourite
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; } = Guid.Empty;
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid ProductId { get; set; } = Guid.Empty;

        /// <summary>
        /// Пользователь
        /// </summary>
        public User User { get; set; } = new User();
        /// <summary>
        /// Товар
        /// </summary>
        public Product Product { get; set; } = new Product();
    }
}
