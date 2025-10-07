using AutoMapper;
using Microsoft.AspNetCore.Components;
using ProductCategory_WebApi.Application.Dtos;
using ProductCategory_WebApi.Domain.Interfaces;
using ProductCategory_WebApi.Domain.Models;

namespace ProductCategory_WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ColorsController : GenericController<Colors, ColorDto>
    {
        public ColorsController(IColorRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
