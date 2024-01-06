using AutoMapper;
using vogue_decor.Application.DTOs.CollectionDTOs;
using vogue_decor.Domain;

namespace vogue_decor.Application.Common.Mappings.MappingProfiles
{
    /// <summary>
    /// Профиль конфигурации маппинга коллекции
    /// </summary>
    public class CollectionMappingProfile : Profile
    {
        public CollectionMappingProfile()
        {
            CreateMap<Collection, CollectionLookupDto>(MemberList.Source);
        }
    }
}
