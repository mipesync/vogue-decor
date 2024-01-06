namespace vogue_decor.Application.DTOs.UserDTOs
{
    /// <summary>
    /// DTO для обновления персональных данных пользователя
    /// </summary>
    public class UpdateUserDetailsDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Номер телефона пользователя
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
