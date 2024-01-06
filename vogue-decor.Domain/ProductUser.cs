namespace vogue_decor.Domain
{
    /// <summary>
    /// Класс корзины пользователя
    /// </summary>
    public class ProductUser
    {
        /// <summary>
        /// Идентификатор корзины
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public Guid ProductId { get; set; }

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
