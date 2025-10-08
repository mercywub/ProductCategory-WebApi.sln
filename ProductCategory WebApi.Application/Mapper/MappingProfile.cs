using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProductCategory_WebApi.Application.Dtos;
using ProductCategory_WebApi.Domain.Models;

namespace ProductCategory_WebApi.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           
            CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Colors,ColorDto>().ReverseMap();
            CreateMap<ProductColor, ColorDto>().ReverseMap();
            CreateMap<ProductGallery, GalleryDto>().ReverseMap();
            CreateMap<ProductSize,SizeDto>().ReverseMap();
            CreateMap<Sizes,SizeDto>().ReverseMap();
            CreateMap<Gallary,GalleryDto>().ReverseMap();
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()).ReverseMap(); // Hash later
            CreateMap<ResetPasswordOtpDto, User>()
           .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.NewPassword))).
           ReverseMap();
            CreateMap<User,UserDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<Payment, UpdatePaymentDto>().ReverseMap();

            CreateMap<PaymentCreateDto, Payment>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<CreateAddressDto, Address>();
            CreateMap<UpdateAddressDto, Address>();

           


        }
    }
}
