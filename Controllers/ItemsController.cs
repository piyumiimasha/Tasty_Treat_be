using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll()
        {
            var items = await _itemService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetById(int id)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item == null)
                return NotFound($"Item with id {id} not found");

            return Ok(item);
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetByCategory(string category)
        {
            var items = await _itemService.GetByCategoryAsync(category);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Create([FromBody] CreateItemDto createItemDto)
        {
            try
            {
                var item = await _itemService.CreateAsync(createItemDto);
                return CreatedAtAction(nameof(GetById), new { id = item.ItemId }, item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> Update(int id, [FromBody] UpdateItemDto updateItemDto)
        {
            try
            {
                var item = await _itemService.UpdateAsync(id, updateItemDto);
                return Ok(item);
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
            var result = await _itemService.DeleteAsync(id);
            if (!result)
                return NotFound($"Item with id {id} not found");

            return NoContent();
        }
    }
}
