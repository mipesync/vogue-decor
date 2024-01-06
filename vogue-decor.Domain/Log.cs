using vogue_decor.Domain.Enums;

namespace vogue_decor.Domain
{
    /// <summary>
    /// Класс лога
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Идентификатор лога
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; } = Guid.Empty;
        /// <summary>
        /// Сообщение лога
        /// </summary>
        public string Message { get; set; } = string.Empty;
        /// <summary>
        /// Тип лога
        /// </summary>
        public LogType Type { get; set; } = LogType.NONE;
        /// <summary>
        /// Дата лога
        /// </summary>
        public DateTime Date { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Пользователь, к которому привязан лог
        /// </summary>
        public User User { get; set; } = new User();
    }
}
