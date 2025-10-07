using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductCategory_WebApi.Domain.Interfaces;

namespace ProductCategory_WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<TEntity, TDto> : ControllerBase
    where TEntity : class
    where TDto : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public GenericController(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
            return Ok(dtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return NotFound();
            var dto = _mapper.Map<TDto>(entity);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            var resultDto = _mapper.Map<TDto>(entity);
            return CreatedAtAction(nameof(Get), new { id = GetEntityId(entity) }, resultDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var entity = _mapper.Map(dto, existing);
            SetEntityId(entity, id);
            await _repository.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        private Guid GetEntityId(TEntity entity)
        {
            var prop = typeof(TEntity).GetProperty("Id");
            return (Guid)prop.GetValue(entity);
        }

        private void SetEntityId(TEntity entity, Guid id)
        {
            var prop = typeof(TEntity).GetProperty("Id");
            prop.SetValue(entity, id);
        }
    }
}
