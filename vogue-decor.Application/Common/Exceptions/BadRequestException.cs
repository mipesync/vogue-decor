namespace vogue_decor.Application.Common.Exceptions
{
    /// <summary>
    /// Исключение некорректных данных
    /// </summary>
    public class BadRequestException : Exception
    {
        /// <summary>
        /// Инициализация исключения с кастомным сообщением
        /// </summary>
        /// <param name="message">Сообщение исключения</param>
        public BadRequestException(string message) : base(message) { }
    }
}
