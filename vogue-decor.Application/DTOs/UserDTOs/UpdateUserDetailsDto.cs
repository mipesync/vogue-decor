using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Guid UserId { get; set; } = Guid.Empty;
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
