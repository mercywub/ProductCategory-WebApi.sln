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
            
        }
    }
}
