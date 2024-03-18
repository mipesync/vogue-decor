using vogue_decor.Application.DTOs.UserDTOs;
using vogue_decor.Application.DTOs.UserDTOs.ResponseDTOs;

namespace vogue_decor.Application.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория пользователя
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <returns><see cref="GetUsersResponseDto"/></returns>
        Task<GetUsersResponseDto> GetAll();
        /// <summary>
        /// Получить информацию о пользователе по идентификатору
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <returns><see cref="UserResponseDto"/></returns>
        Task<UserResponseDto> GetById(GetUserByIdDto dto);
        /// <summary>
        /// Получить корзину
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <param name="hostUrl">Домен API</param>
        /// <returns><see cref="GetCartResponseDto"/></returns>
        Task<GetCartResponseDto> GetCart(GetUserByIdDto dto, string hostUrl);
        /// <summary>
        /// Очистить корзину
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        Task ClearCart(Guid userId);
        /// <summary>
        /// Получить избранные
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <param name="hostUrl">Домен API</param>
        /// <returns><see cref="GetCartResponseDto"/></returns>
        Task<GetCartResponseDto> GetFavourites(GetUserByIdDto dto, string hostUrl);
        /// <summary>
        /// Изменить персональные данные пользователя
        /// </summary>
        /// <param name="dto">Входные данные</param>
        Task UpdateDetails(UpdateUserDetailsDto dto);
        /// <summary>
        /// Оформить заказ
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <param name="hostUrl">Домен API</param>
        Task OrderPlace(OrderPlaceDto dto, string hostUrl);
        /// <summary>
        /// Обратная связь
        /// </summary>
        /// <param name="dto">Входные данные</param>
        Task Feedback(FeedbackDto dto);
    }
}
