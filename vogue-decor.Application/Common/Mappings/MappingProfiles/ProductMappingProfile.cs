using AutoMapper;
using vogue_decor.Application.Common.Services;
using vogue_decor.Application.DTOs.ProductDTOs;
using vogue_decor.Application.DTOs.UserDTOs.ResponseDTOs;
using vogue_decor.Domain;
using vogue_decor.Domain.Enums;
using Microsoft.IdentityModel.Tokens;
using vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs;

namespace vogue_decor.Application.Common.Mappings.MappingProfiles
{
    /// <summary>
    /// Профиль конфигурации маппинга товара
    /// </summary>
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<CreateProductDto, Product>(MemberList.Source)
                .ForMember(product => product.Id, opt => Guid.NewGuid())
                .ForMember(product => product.ProductType, opt => opt.MapFrom(dto => dto.Type))
                .ForMember(product => product.Types, opt => opt.MapFrom(dto => dto.Categories));

            CreateMap<UpdateProductDto, Product>(MemberList.Source)
                .ForMember(product => product.Id, opt => opt.MapFrom(u => u.ProductId))
                .ForMember(dest => dest.Name, opt => opt.Condition(u => !u.Name.IsNullOrEmpty()))
                .ForMember(dest => dest.Description, opt => opt.Condition(u => !u.Description.IsNullOrEmpty()))
                .ForMember(dest => dest.ProductType, opt => opt.Condition(u => u.Type != null))
                .ForMember(dest => dest.Article, opt => opt.Condition(u => !u.Article.IsNullOrEmpty()))
                .ForMember(dest => dest.Price, opt => opt.Condition(u => u.Price != null))
                .ForMember(dest => dest.Colors, opt => opt.Condition(u => u.Colors != null))
                .ForMember(dest => dest.Diameter, opt => opt.Condition(u => u.Diameter != null))
                .ForMember(dest => dest.Height, opt => opt.Condition(u => u.Height != null))
                .ForMember(dest => dest.Length, opt => opt.Condition(u => u.Length != null))
                .ForMember(dest => dest.Width, opt => opt.Condition(u => u.Width != null))
                .ForMember(dest => dest.Plinth, opt => opt.Condition(u => !u.Plinth.IsNullOrEmpty()))
                .ForMember(dest => dest.LampCount, opt => opt.Condition(u => u.LampCount != null))
                .ForMember(dest => dest.Availability, opt => opt.Condition(u => u.Availability != null))
                .ForMember(dest => dest.CollectionId, opt => opt.Condition(u => u.CollectionId != null))
                .ForMember(dest => dest.Types, opt => opt.Condition(u => u.Categories != null))
                .ForMember(dest => dest.Discount, opt => opt.Condition(u => u.Discount != null))
                .ForMember(dest => dest.Styles, opt => opt.Condition(u => u.Styles != null))
                .ForMember(dest => dest.Materials, opt => opt.Condition(u => u.Materials != null))
                .ForMember(dest => dest.BrandId, opt => opt.Condition(u => u.BrandId != null))
                .ForMember(dest => dest.PictureMaterial, opt => opt.Condition(u => u.PictureMaterial != null))
                .ForMember(dest => dest.Indent, opt => opt.Condition(u => u.Indent != null))
                .ForMember(dest => dest.ChandelierTypes, opt => opt.Condition(u => u.ChandelierTypes != null));

            CreateMap<Product, ProductShortResponseDto>(MemberList.Source)
                .ForMember(dto => dto.IsSale, opt => opt.MapFrom((product, _, _, _) => product.Discount is not (0 or null)))
                .ForMember(dto => dto.IsFavourite, opt =>
                    opt.MapFrom((product, _, _, context) =>
                    {
                        if (Guid.Parse(context.Items["userId"].ToString()!) == Guid.Empty)
                            return false;

                        return ProductStatesChecker
                            .Check(Guid.Parse(context.Items["userId"].ToString()!), product)
                            .IsFavourite;
                    }))
                .ForMember(dto => dto.IsCart, opt =>
                    opt.MapFrom((product, dto, isCart, context) =>
                    {
                        if (Guid.Parse(context.Items["userId"].ToString()!) == Guid.Empty)
                            return false;

                        return ProductStatesChecker
                            .Check(Guid.Parse(context.Items["userId"].ToString()!), product)
                            .IsCart;
                    }))
                .ForMember(dto => dto.Quantity, opt => opt.MapFrom(product => product.ProductUsers.Count));

            CreateMap<Product, ProductResponseDto>(MemberList.Source)
                .ForMember(dto => dto.Collection, opt => opt.Ignore())
                .ForMember(dto => dto.Brand, opt => opt.Ignore())
                .ForMember(dto => dto.IsFavourite, opt =>
                    opt.MapFrom((product, dto, isFavourite, context) =>
                    {
                        if (Guid.Parse(context.Items["userId"].ToString()!) == Guid.Empty)
                            return false;

                        return ProductStatesChecker
                            .Check(Guid.Parse(context.Items["userId"].ToString()!), product)
                            .IsFavourite;
                    }))
                .ForMember(dto => dto.IsCart, opt =>
                    opt.MapFrom((product, dto, isCart, context) =>
                    {
                        if (Guid.Parse(context.Items["userId"].ToString()!) == Guid.Empty)
                            return false;

                        return ProductStatesChecker
                            .Check(Guid.Parse(context.Items["userId"].ToString()!), product)
                            .IsCart;
                    }))
                .ForMember(dto => dto.Files, opt => opt.MapFrom((product, dto, _files, context) =>
                {
                    var files = new List<FileDto>();

                    foreach (var url in product.Urls)
                    {
                        files.Add(new FileDto
                        {
                            Url = url,
                            Name = url
                        });
                    }

                    return files;
                }))
                .ForMember(dto => dto.Quantity, opt => opt.MapFrom(product => product.ProductUsers.Count))
                .ForMember(dto => dto.Code, opt => opt.MapFrom(product => long.Parse(product.Code)));
            
            CreateMap<ProductResponseDto, ProductShortResponseDto>(MemberList.Source);
            CreateMap<Product, CartResponseDto>(MemberList.Source)
                .ForMember(dto => dto.Length, opt => opt.MapFrom(prod => prod.Length))
                .ForMember(dto => dto.IsFavourite, opt =>
                    opt.MapFrom((product, dto, isFavourite, context) =>
                    {
                        if (Guid.Parse(context.Items["userId"].ToString()!) == Guid.Empty)
                            return false;

                        return ProductStatesChecker
                            .Check(Guid.Parse(context.Items["userId"].ToString()!), product)
                            .IsFavourite;
                    }))
                .ForMember(dto => dto.IsCart, opt =>
                    opt.MapFrom((product, dto, isCart, context) =>
                    {
                        if (Guid.Parse(context.Items["userId"].ToString()!) == Guid.Empty)
                            return false;

                        return ProductStatesChecker
                            .Check(Guid.Parse(context.Items["userId"].ToString()!), product)
                            .IsCart;
                    }))
                .ForMember(dto => dto.Files, opt => opt.MapFrom((product, dto, _files, context) =>
                {
                    var files = new List<FileDto>();

                    foreach (var url in product.Urls)
                    {
                        files.Add(new FileDto
                        {
                            Url = url,
                            Name = url
                        });
                    }

                    return files;
                }))
                .ForMember(dto => dto.Quantity, opt => opt.MapFrom(product => product.ProductUsers.Count))
                .ForMember(dto => dto.Code, opt => opt.MapFrom(product => long.Parse(product.Code)));
            CreateMap<Product, CartResponseDto>(MemberList.Source)
                .ForMember(dto => dto.Collection, opt => opt.Ignore())
                .ForMember(dto => dto.Brand, opt => opt.Ignore());
        }
    }
}
