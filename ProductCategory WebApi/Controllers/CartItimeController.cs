using AutoMapper;
using Microsoft.AspNetCore.Components;
using ProductCategory_WebApi.Application.Dtos;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Domain.Models;

namespace ProductCategory_WebApi.Controllers
{

    [Route("api/[controller]")]
    public class CartItemController : GenericController<Cart, CartDto>
    {
        public CartItemController(ICartRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
