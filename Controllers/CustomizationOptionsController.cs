using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomizationOptionsController : ControllerBase
    {
        private readonly ICustomizationOptionService _customizationOptionService;

        public CustomizationOptionsController(ICustomizationOptionService customizationOptionService)
        {
            _customizationOptionService = customizationOptionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomizationOptionDto>>> GetAll()
        {
            var options = await _customizationOptionService.GetAllAsync();
            return Ok(options);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomizationOptionDto>> GetById(int id)
        {
            var option = await _customizationOptionService.GetByIdAsync(id);
            if (option == null)
                return NotFound($"CustomizationOption with id {id} not found");

            return Ok(option);
        }

        [HttpGet("item/{itemId}")]
        public async Task<ActionResult<IEnumerable<CustomizationOptionDto>>> GetByItemId(int itemId)
        {
            var options = await _customizationOptionService.GetByItemIdAsync(itemId);
            return Ok(options);
        }

        [HttpPost]
        public async Task<ActionResult<CustomizationOptionDto>> Create([FromBody] CreateCustomizationOptionDto createCustomizationOptionDto)
        {
            try
            {
                var option = await _customizationOptionService.CreateAsync(createCustomizationOptionDto);
                return CreatedAtAction(nameof(GetById), new { id = option.OptionId }, option);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomizationOptionDto>> Update(int id, [FromBody] UpdateCustomizationOptionDto updateCustomizationOptionDto)
        {
            try
            {
                var option = await _customizationOptionService.UpdateAsync(id, updateCustomizationOptionDto);
                return Ok(option);
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
            var result = await _customizationOptionService.DeleteAsync(id);
            if (!result)
                return NotFound($"CustomizationOption with id {id} not found");

            return NoContent();
        }
    }
}
