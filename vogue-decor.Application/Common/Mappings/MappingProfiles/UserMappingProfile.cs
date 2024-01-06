using AutoMapper;
using vogue_decor.Application.DTOs.UserDTOs;
using vogue_decor.Application.DTOs.UserDTOs.ResponseDTOs;
using vogue_decor.Domain;

namespace vogue_decor.Application.Common.Mappings.MappingProfiles
{
    /// <summary>
    /// Профиль конфигурации маппинга пользователя
    /// </summary>
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserShortResponseDto>(MemberList.Source);
            CreateMap<User, UserResponseDto>(MemberList.Source);
            CreateMap<UpdateUserDetailsDto, User>(MemberList.Source)
                .ForMember(user => user.Id, opt => opt.MapFrom(dto => dto.UserId));
        }
    }
}
