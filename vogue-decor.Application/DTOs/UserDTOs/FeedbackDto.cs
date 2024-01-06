namespace vogue_decor.Application.DTOs.UserDTOs
{
    /// <summary>
    /// DTO для обратной связи
    /// </summary>
    public class FeedbackDto
    {
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Содержание обращения
        /// </summary>
        public string Text { get; set; } = string.Empty;
    }
}
