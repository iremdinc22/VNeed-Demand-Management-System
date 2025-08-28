using System.ComponentModel.DataAnnotations;
using System.Reflection;
using AutoMapper;
using Vneed.Data.Models;
using Vneed.Services.Dto;

namespace Vneed.Service;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
            
        CreateMap<Category, CategoryDto>().ReverseMap();
           
        CreateMap<Role, RoleDto>().ReverseMap();
            
        CreateMap<User, UserDto>().ReverseMap();
            
        CreateMap<Demand, DemandDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null));
            
        CreateMap<DemandDto, Demand>()
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());  
            
        CreateMap<UserToken, UserTokenDto>().ReverseMap();
        
        CreateMap<ProductSuggestion, ProductSuggestionDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ReverseMap();

       // CreateMap<ProductSuggestionDto, ProductSuggestion>()
          //  .ForMember(dest => dest.Category, opt => opt.Ignore());

        // Bunu yeni ekledim
       // CreateMap<ProductSuggestion, ProductSuggestionDto>().ReverseMap();
       // CreateMap<ProductSuggestionDto, ProductSuggestion>()
         //   .ForMember(dest => dest.Category, opt => opt.Ignore());
         
         CreateMap<ProductSuggestion, ProductSuggestionDto>()
             .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

         CreateMap<ProductSuggestionDto, ProductSuggestion>()
             .ForMember(dest => dest.Category, opt => opt.Ignore());
         

        CreateMap<Demand, DemandDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null))
            .ForMember(dest => dest.StatusText, opt => opt.Ignore()) // Önce ignore
            .AfterMap((src, dest) =>
            {
                var field = src.Status.GetType().GetField(src.Status.ToString());
                var displayAttr = field?.GetCustomAttribute<DisplayAttribute>();
                dest.StatusText = displayAttr?.Name ?? src.Status.ToString();
            }); 
    }
}