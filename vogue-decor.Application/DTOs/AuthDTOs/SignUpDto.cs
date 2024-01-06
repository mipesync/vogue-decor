using System.ComponentModel.DataAnnotations;

namespace vogue_decor.Application.DTOs.AuthDTOs
{
    /// <summary>
    /// DTO для регистрации
    /// </summary>
    public class SignUpDto
    {
        /// <summary>
        /// Почта пользователя
        /// </summary>
        [Required(ErrorMessage = "Почта обязательна")]
        [EmailAddress(ErrorMessage = "Некорректная почта")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(8, ErrorMessage = "Длина пароля должна быть не меньше 8 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
