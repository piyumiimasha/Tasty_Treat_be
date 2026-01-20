using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetAll()
        {
            var orderItems = await _orderItemService.GetAllAsync();
            return Ok(orderItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemDto>> GetById(int id)
        {
            var orderItem = await _orderItemService.GetByIdAsync(id);
            if (orderItem == null)
                return NotFound($"OrderItem with id {id} not found");

            return Ok(orderItem);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetByOrderId(int orderId)
        {
            var orderItems = await _orderItemService.GetByOrderIdAsync(orderId);
            return Ok(orderItems);
        }

        [HttpPost]
        public async Task<ActionResult<OrderItemDto>> Create([FromBody] CreateOrderItemDto createOrderItemDto)
        {
            try
            {
                var orderItem = await _orderItemService.CreateAsync(createOrderItemDto);
                return CreatedAtAction(nameof(GetById), new { id = orderItem.OrderItemId }, orderItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderItemDto>> Update(int id, [FromBody] UpdateOrderItemDto updateOrderItemDto)
        {
            try
            {
                var orderItem = await _orderItemService.UpdateAsync(id, updateOrderItemDto);
                return Ok(orderItem);
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
            var result = await _orderItemService.DeleteAsync(id);
            if (!result)
                return NotFound($"OrderItem with id {id} not found");

            return NoContent();
        }
    }
}
