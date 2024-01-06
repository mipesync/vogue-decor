using System.Security.Claims;
using vogue_decor.Application.Common.Attributes;
using vogue_decor.Application.DTOs.CollectionDTOs.ResponseDTOs;
using vogue_decor.Application.DTOs.UserDTOs;
using vogue_decor.Application.DTOs.UserDTOs.ResponseDTOs;
using vogue_decor.Application.Interfaces.Repositories;
using vogue_decor.Domain.Enums;
using vogue_decor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using vogue_decor.Attributes;

namespace vogue_decor.Controllers
{
    [Route("api/user")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private Guid UserId
        {
            get
            {
                var claimNameId = Request.HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);
                if (claimNameId is null)
                    return Guid.Empty;
            
                return Guid.Parse(claimNameId.Value);
            }
        }
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Получить список всех пользователей
        /// </summary>
        /// <returns><see cref="GetUsersResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(GetUsersResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userRepository.GetAll();

            return Ok(result);
        }

        /// <summary>
        /// Получить пользователя по идентификатору
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <returns><see cref="UserResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("details")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(UserResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> GetById([FromQuery] GetUserByIdDto dto)
        {
            var result = await _userRepository.GetById(dto);

            return Ok(result);
        }

        /// <summary>
        /// Получить корзину пользователя
        /// </summary>
        /// <returns><see cref="UserResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("cart")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(UserResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> GetCart()
        {
            var result = await _userRepository.GetCart(new GetUserByIdDto { UserId = UserId});

            return Ok(result);
        }

        /// <summary>
        /// Очистить корзину пользователя
        /// </summary>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("cart")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> ClearCart()
        {
            await _userRepository.ClearCart(UserId);

            return Ok();
        }

        /// <summary>
        /// Получить избранные пользователя
        /// </summary>
        /// <returns><see cref="UserResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("favourites")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(UserResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> GetFavourites()
        {
            var result = await _userRepository.GetFavourites(new GetUserByIdDto { UserId = UserId});

            return Ok(result);
        }

        /// <summary>
        /// Обновить персональные данные пользователя
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPut("details")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> UpdateDetails([FromBody] UpdateUserDetailsDto dto)
        {
            await _userRepository.UpdateDetails(dto);

            return Ok();
        }

        /// <summary>
        /// Оформить заказ
        /// </summary>
        /// <remarks>
        /// Присылает на корпоративную почту и почту пользователя сообщение с составом 
        /// заказа
        /// </remarks>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("order")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> OrderPlace([FromQuery] OrderPlaceDto dto)
        {
            await _userRepository.OrderPlace(dto);

            return Ok();
        }

        /// <summary>
        /// Обратная связь
        /// </summary>
        /// <remarks>
        /// Присылает на корпоративную почту письмо с обращением пользователя с его данными
        /// </remarks>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("feedback")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> Feedback([FromQuery] FeedbackDto dto)
        {
            await _userRepository.Feedback(dto);

            return Ok();
        }
    }
}
