using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using ProductCategory_WebApi.Application.Dtos;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Domain.Models;

namespace ProductCategory_WebApi.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    public class ProductController : GenericController<Product, ProductDto>
    {
        public ProductController(IGenericRepository<Product> repository, IMapper mapper)
            : base(repository, mapper) { }
    }
}
