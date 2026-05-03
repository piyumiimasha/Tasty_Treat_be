using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomizationTypesController : ControllerBase
    {
        private readonly ICustomizationTypeService _service;

        public CustomizationTypesController(ICustomizationTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomizationTypeDto>>> GetAll()
        {
            var types = await _service.GetAllAsync();
            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomizationTypeDto>> GetById(int id)
        {
            var type = await _service.GetByIdAsync(id);
            if (type == null) return NotFound($"CustomizationType with id {id} not found");
            return Ok(type);
        }

        [HttpPost]
        public async Task<ActionResult<CustomizationTypeDto>> Create([FromBody] CreateCustomizationTypeDto dto)
        {
            try
            {
                var type = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = type.TypeId }, type);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomizationTypeDto>> Update(int id, [FromBody] UpdateCustomizationTypeDto dto)
        {
            try
            {
                var type = await _service.UpdateAsync(id, dto);
                return Ok(type);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound($"CustomizationType with id {id} not found");
            return NoContent();
        }
    }
}
