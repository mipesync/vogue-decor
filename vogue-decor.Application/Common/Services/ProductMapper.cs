using AutoMapper;
using vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs;
using vogue_decor.Domain;

namespace vogue_decor.Application.Common.Services
{
    /// <summary>
    /// Позволяет смаппить список <see cref="Product"/> в список <see cref="ProductShortResponseDto"/>
    /// </summary>
    public static class ProductMapper
    {
        /// <summary>
        /// Преобразует список <see cref="Product"/> в список <see cref="ProductShortResponseDto"/>
        /// </summary>
        /// <param name="mapper">Маппер</param>
        /// <param name="products">Список Товаров</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        public static List<ProductShortResponseDto> Map(IMapper mapper, List<Product> products, Guid userId)
        {
            var dtos = new List<ProductShortResponseDto>();

            foreach (var entity in products)
            {
                var dto = mapper.Map<ProductShortResponseDto>(entity, opt =>
                {
                    opt.Items["userId"] = userId;
                });

                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
