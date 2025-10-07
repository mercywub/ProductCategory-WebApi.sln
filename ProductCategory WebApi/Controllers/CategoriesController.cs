using AutoMapper;
using Microsoft.AspNetCore.Components;
using ProductCategory_WebApi.Application.Dtos;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Domain.Models;
using ProductCategory_WebApi.Infrastructure.Repositories;

namespace ProductCategory_WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : GenericController<ProductCategory, ProductCategoryDto>
    {
        public CategoriesController(IProductCategory repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
