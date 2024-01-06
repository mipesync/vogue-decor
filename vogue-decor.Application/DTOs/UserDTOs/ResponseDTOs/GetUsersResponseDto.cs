﻿namespace vogue_decor.Application.DTOs.UserDTOs.ResponseDTOs
{
    /// <summary>
    /// DTO, возвращаемое из метода получения списка пользователей
    /// </summary>
    public class GetUsersResponseDto
    {
        /// <summary>
        /// Список пользователей
        /// </summary>
        public List<UserShortResponseDto> Users { get; set; } = new List<UserShortResponseDto>();
    }
}
