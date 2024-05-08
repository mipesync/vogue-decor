using System.Text.Json.Serialization;

namespace vogue_decor.Application.DTOs.UserDTOs
{
    /// <summary>
    /// DTO для оформления заказа
    /// </summary>
    public class OrderPlaceDto
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
        /// <summary>
        /// Комментарий к заказу
        /// </summary>
        public string Comment { get; set; } = string.Empty;
    }
}
