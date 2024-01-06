using vogue_decor.Application.DTOs;
using vogue_decor.Domain;

namespace vogue_decor.Application.Common.Services
{
    /// <summary>
    /// Позволяет проверить, оценён ли альбом текущим пользователем
    /// </summary>
    public static class ProductStatesChecker
    {
        /// <summary>
        /// Проверяет, оценён ли альбом текущим пользователем
        /// </summary>
        /// <param name="userId">Идентификатор текущего пользователя</param>
        /// <param name="product">Товар</param>
        /// <returns><see cref="StatesCheckerDto"/></returns>
        public static StatesCheckerDto Check(Guid userId, Product product)
        {
            var result = new StatesCheckerDto();

            if (product.Favourites!.Any(u => u.UserId == userId))
                result.IsFavourite = true;

            if (product.ProductUsers!.Any(u => u.UserId == userId))
                result.IsCart = true;

            return result;
        }
    }
}
