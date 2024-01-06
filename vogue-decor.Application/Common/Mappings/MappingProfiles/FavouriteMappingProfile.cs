using AutoMapper;
using vogue_decor.Application.DTOs.ProductDTOs;
using vogue_decor.Domain;

namespace vogue_decor.Application.Common.Mappings.MappingProfiles
{
    /// <summary>
    /// Профиль конфигурации избранных
    /// </summary>
    public class FavouriteMappingProfile : Profile
    {
        public FavouriteMappingProfile()
        {
            CreateMap<AddToFavouriteDto, Favourite>(MemberList.Source);

            CreateMap<RemoveFromFavouriteDto, Favourite>(MemberList.Source);
        }
    }
}
