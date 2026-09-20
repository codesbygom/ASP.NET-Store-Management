using AutoMapper;
using Store.Application.DTOs.Authentication;
using Store.Application.DTOs.Category;
using Store.Application.DTOs.Customer;
using Store.Application.DTOs.Order;
using Store.Application.DTOs.Product;
using Store.Domain.Entities;

namespace Store.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region customer
            CreateMap<Customer, CustomerDTO>();

            CreateMap<UpsertCustomer, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Register, Customer>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            #endregion

            #region category
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.TotalProducts, opt => opt.MapFrom(src => src.Products == null ? 0 : src.Products.Count));

            CreateMap<UpsertCategory, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            #endregion

            #region product
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category == null ? string.Empty : src.Category.Title));

            CreateMap<UpsertProduct, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            #endregion

            #region order
            CreateMap<Order, OrderDTO>();

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product == null ? string.Empty : src.Product.Title));
            #endregion
        }
    }
}
