using vogue_decor.Application.DTOs.AuthDTOs;
using vogue_decor.Application.DTOs.AuthDTOs.ResponseDTOs;
using vogue_decor.Application.Interfaces.Repositories;
using vogue_decor.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;

namespace vogue_decor.Controllers
{
    /// <summary>
    /// Контроллер авторизации
    /// </summary>
    [Route("api/auth")]
    [Produces("application/json")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        /// <summary>
        /// Инициализация начальных параметров
        /// </summary>
        /// <param name="authRepository">Репозиторий авторизации</param>
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <remarks>
        /// Если аккаунт пользователя не подтверждён, то зайти в него он не сможет
        /// </remarks>
        /// <param name="dto">Входные авторизационные данные</param>
        /// <returns><see cref="SignInResponseApiDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Неверный пароль</response>
        /// <response code="400">Неудачная попытка входа</response>
        /// <response code="403">Почта не подтверждена</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("sign-in")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(SignInResponseApiDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status403Forbidden, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> SignIn([FromBody] SignInDto dto)
        {
            var result = await _authRepository.SignIn(dto);
            
            /*if (result.refresh_token is not null)
                Response.Cookies.Append("refresh_token", result.refresh_token, new CookieOptions() { SameSite = SameSiteMode.Lax });*/
            
            return Ok(new SignInResponseDto
            {
                userId = result.userId,
                access_token = result.access_token,
                access_token_expires = result.access_token_expires,
                refresh_token = result.refresh_token,
                refresh_token_expires = result.refresh_token_expires
            });
        }

        /// <summary>
        /// Обновление токена доступа
        /// </summary>
        /// <returns><see cref="RefreshTokenResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Недействительный токен</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("token-refresh")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(RefreshTokenResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> TokenRefresh([FromQuery] string refreshToken)
        {
            //var refreshToken = Request.Cookies.FirstOrDefault(u => u.Key == "refresh_token").Value;
            var result = await _authRepository.RefreshToken(refreshToken);

            //Response.Cookies.Append("refresh_token", result.refresh_token, new CookieOptions() { SameSite = SameSiteMode.Lax });
            
            return Ok(new RefreshTokenResponseDto
            {
                userId = result.userId,
                access_token = result.access_token,
                access_token_expires = result.access_token_expires,
                refresh_token = result.refresh_token,
                refresh_token_expires = result.refresh_token_expires
            });
        }

        /// <summary>
        /// Регистрация администратора
        /// </summary>
        /// <remarks>
        /// Пароль должен состоять из верхнего и нижнего регистра, содержать специальные символы и цифры <br/><br/>
        /// После регистрации на корпоративную почту присылается письмо с кодом для подтверждения 
        /// регистрации нового администратора. Если этот человек действительно является новым администратором, 
        /// то ему сообщается код и он завершает регистрацию. <br/>
        /// В случае, если это "незнакомец", то просто игнорируем сообщение, т.к. без подтверждения почты 
        /// в свой аккаунт он зайти не сможет
        /// </remarks>
        /// <param name="dto">Входные регистрационные данные</param>
        /// <returns><see cref="SignUpResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Пользователь с такой почтой уже существует</response>
        /// <response code="404">Что-то пошло не так</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("admin-sign-up")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(SignUpResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> AdminSignUp([FromBody] SignUpDto dto)
        {
            var result = await _authRepository.AdminSignUp(dto);

            return Ok(result);
        }

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <remarks>
        /// Пароль должен состоять из верхнего и нижнего регистра, содержать специальные символы и цифры <br/><br/>
        /// После регистрации на почту пользователя присылается письмо с кодом для подтверждения.
        /// </remarks>
        /// <param name="dto">Входные регистрационные данные</param>
        /// <returns><see cref="SignUpResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Пользователь с такой почтой уже существует</response>
        /// <response code="404">Что-то пошло не так</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("sign-up")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(SignUpResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto dto)
        {
            var result = await _authRepository.SignUp(dto);

            return Ok(result);
        }

        /// <summary>
        /// Подтверждение почты
        /// </summary>
        /// <remarks>
        /// После регистрации, код подтверждения присылается на этот эндпоинт для завершения регистрации
        /// </remarks>
        /// <param name="dto">Входные данные подтверждения почты</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Неправильный код подтверждения</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("email-confirm")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> EmailConfirmation([FromBody] ConfirmEmailDto dto)
        {
            await _authRepository.ConfirmEmail(dto);

            return Ok();
        }

        /// <summary>
        /// Повторная отправка кода подтверждения
        /// </summary>
        /// <remarks>
        /// В случае, если пользователь так и не получил письмо подтверждения, его нужно пислать повторно 
        /// при помощи этого эндпоита
        /// </remarks>
        /// <param name="dto">Входные данные для повторной отправки кода подтверждения</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("email-confirm-resend")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(RefreshTokenResponseApiDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> EmailResendConfimation([FromQuery] ResendEmailConfirmationDto dto)
        {
            await _authRepository.ResendEmailConfirmation(dto);

            return Ok();
        }

        /// <summary>
        /// Запрос кода подтверждения для сброса пароля
        /// </summary>
        /// <remarks>
        /// Так называемый "Забыл пароль". После выполнения пользователю приходит письмо подтверждения сброса пароля 
        /// с кодом
        /// </remarks>
        /// <param name="dto">Входные данные для запроса кода подтверждения</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("password-forgot")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(RefreshTokenResponseApiDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> PasswordForgot([FromQuery] ForgotPasswordDto dto)
        {
            await _authRepository.ForgotPassword(dto);

            return Ok();
        }

        /// <summary>
        /// Сброс пароля
        /// </summary>
        /// <remarks>
        /// Тут выполняется сама операция по смене пароля пользователя
        /// </remarks>
        /// <param name="dto">Входные данные для сброса пароля</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Неправильный код подтверждения</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="404">Что-то пошло не так</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("password-reset")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(RefreshTokenResponseApiDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> PasswordReset([FromBody] ResetPasswordDto dto)
        {
            await _authRepository.ResetPassword(dto);

            return Ok();
        }
    }
}
