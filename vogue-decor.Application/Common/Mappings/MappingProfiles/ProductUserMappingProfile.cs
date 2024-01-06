using AutoMapper;
using vogue_decor.Application.DTOs.ProductDTOs;
using vogue_decor.Domain;

namespace vogue_decor.Application.Common.Mappings.MappingProfiles
{
    /// <summary>
    /// Профиль конфигурации маппинга корзины
    /// </summary>
    public class ProductUserMappingProfile : Profile
    {
        public ProductUserMappingProfile()
        {
            CreateMap<AddToCartDto, ProductUser>(MemberList.Source)
                .ForMember(product => product.Id, opt => new Guid());

            CreateMap<RemoveFromCartDto, ProductUser>(MemberList.Source)
                .ForMember(product => product.Id, opt => new Guid());
        }
    }
}
