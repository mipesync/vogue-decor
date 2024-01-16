using vogue_decor.Domain;

namespace vogue_decor.Application.Common.Exceptions
{
    /// <summary>
    /// Исключение, когда сущность необходимая сущность не найдена
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Инициализация исключения с кастомным сообщением
        /// </summary>
        /// <param name="message">Сообщение исключения</param>
        public NotFoundException(string message) : base(message) { }
        /// <summary>
        /// Инициализация исключения с сообщением отсутсвия пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        public NotFoundException(User? user) : base("Пользователь не найден") { }
        /// <summary>
        /// Инициализация исключения с сообщением отсутсвия коллекции
        /// </summary>
        /// <param name="collection">Коллекция</param>
        public NotFoundException(Collection? collection) : base("Коллекция не найдена") { }
        /// <summary>
        /// Инициализация исключения с сообщением отсутсвия товара
        /// </summary>
        /// <param name="product">Товар</param>
        public NotFoundException(Product? product) : base("Товар не найден") { }
        /// <summary>
        /// Инициализация исключения с сообщением отсутсвия бренда
        /// </summary>
        /// <param name="brand">Бренд</param>
        public NotFoundException(Brand? brand) : base("Бренд не найден") { }
    }
}
