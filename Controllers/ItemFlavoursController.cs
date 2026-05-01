using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/Items/{itemId}/flavours")]
    [ApiController]
    public class ItemFlavoursController : ControllerBase
    {
        private readonly IItemFlavourService _service;

        public ItemFlavoursController(IItemFlavourService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemFlavourDto>>> GetByItem(int itemId)
        {
            var flavours = await _service.GetByItemIdAsync(itemId);
            return Ok(flavours);
        }

        [HttpPost]
        public async Task<ActionResult<ItemFlavourDto>> Create(int itemId, [FromBody] CreateItemFlavourDto dto)
        {
            try
            {
                var flavour = await _service.CreateAsync(itemId, dto);
                return CreatedAtAction(nameof(GetByItem), new { itemId }, flavour);
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

        [HttpPut("{id}")]
        public async Task<ActionResult<ItemFlavourDto>> Update(int itemId, int id, [FromBody] UpdateItemFlavourDto dto)
        {
            try
            {
                var flavour = await _service.UpdateAsync(id, dto);
                return Ok(flavour);
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
        public async Task<ActionResult> Delete(int itemId, int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound($"Flavour with id {id} not found");

            return NoContent();
        }
    }
}
