using vogue_decor.Domain.Enums;
using vogue_decor.Domain;

namespace vogue_decor.Application.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория логгера
    /// </summary>
    public interface ILogRepository
    {
        /// <summary>
        /// Сохранить лог в базу данных
        /// </summary>
        /// <param name="user">Пользователь, к которому относится лог</param>
        /// <param name="message">Текст сообщеия</param>
        /// <param name="type">Тип лога</param>
        /// <returns></returns>
        Task SaveAsync(User user, string message, LogType type);
    }
}
