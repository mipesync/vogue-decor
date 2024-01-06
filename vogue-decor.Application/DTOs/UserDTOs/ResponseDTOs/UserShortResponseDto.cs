using vogue_decor.Domain.Enums;

namespace vogue_decor.Application.DTOs.UserDTOs.ResponseDTOs
{
    /// <summary>
    /// DTO, возвращаемое из метода получения пользователя
    /// </summary>
    public class UserShortResponseDto
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Роль пользователя
        /// </summary>
        public Roles Role { get; set; } = Roles.NONE;
    }
}
