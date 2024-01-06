using vogue_decor.Application.DTOs.AuthDTOs;
using vogue_decor.Application.DTOs.AuthDTOs.ResponseDTOs;

namespace vogue_decor.Application.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория авторизации
    /// </summary>
    public interface IAuthRepository
    {
        /// <summary>
        /// Регистрация администратора
        /// </summary>
        /// <param name="dto">DTO входных регистрационных данных</param>
        /// <returns><see cref="SignUpResponseDto"/>, а так же письмо на корпоративную 
        /// почту с кодом подтверждения регистрации</returns>
        Task<SignUpResponseDto> AdminSignUp(SignUpDto dto);

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="dto">DTO входных авторизационных данных</param>
        /// <returns><see cref="SignInResponseApiDto"/></returns>
        Task<SignInResponseApiDto> SignIn(SignInDto dto);

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="dto">DTO входных регистрационных данных</param>
        /// <returns><see cref="SignUpResponseDto"/>, а так же письмо на почту
        /// с кодом подтверждения регистрации</returns>
        Task<SignUpResponseDto> SignUp(SignUpDto dto);

        /// <summary>
        /// Подтверждение смены пароля
        /// </summary>
        /// <param name="dto">DTO входных данных для подтверждения смены пароля</param>
        /// <returns>Письмо на почту с ссылкой для сброса пароля</returns>
        Task ForgotPassword(ForgotPasswordDto dto);

        /// <summary>
        /// Смена пароля
        /// </summary>
        /// <param name="dto">DTO входных данных для смены пароля</param>
        /// <returns></returns>
        Task ResetPassword(ResetPasswordDto dto);

        /// <summary>
        /// Подтверждение почты
        /// </summary>
        /// <param name="dto">DTO входных данных для подтверждения почты</param>
        /// <returns></returns>
        Task ConfirmEmail(ConfirmEmailDto dto);

        /// <summary>
        /// Повторная отправка письма подтверждения почты
        /// </summary>
        /// <param name="dto">DTO входных данных для подтверждения почты</param>
        /// <returns></returns>
        Task ResendEmailConfirmation(ResendEmailConfirmationDto dto);

        /// <summary>
        /// Обновление токена обновления
        /// </summary>
        /// <param name="refresh">Старый токен обновления</param>
        /// <returns></returns>
        Task<RefreshTokenResponseApiDto> RefreshToken(string refresh);
    }
}
