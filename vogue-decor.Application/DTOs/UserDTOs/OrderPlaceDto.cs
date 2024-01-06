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
        public Guid UserId { get; set; }
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
