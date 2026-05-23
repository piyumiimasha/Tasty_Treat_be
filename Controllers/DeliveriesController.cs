using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveriesController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<DeliveryDto>>> GetAll()
        {
            var deliveries = await _deliveryService.GetAllAsync();
            return Ok(deliveries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryDto>> GetById(int id)
        {
            var delivery = await _deliveryService.GetByIdAsync(id);
            if (delivery == null)
                return NotFound($"Delivery with id {id} not found");

            return Ok(delivery);
        }

        [HttpGet("delivery-person/{deliveryPersonId}")]
        [Authorize(Roles = "DeliveryPerson,Admin")]
        public async Task<ActionResult<IEnumerable<DeliveryDto>>> GetByDeliveryPersonId(int deliveryPersonId)
        {
            var deliveries = await _deliveryService.GetByDeliveryPersonIdAsync(deliveryPersonId);
            return Ok(deliveries);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<DeliveryDto>> GetByOrderId(int orderId)
        {
            var delivery = await _deliveryService.GetByOrderIdAsync(orderId);
            if (delivery == null)
                return NotFound($"Delivery for order {orderId} not found");

            return Ok(delivery);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DeliveryDto>> Create([FromBody] CreateDeliveryDto createDeliveryDto)
        {
            try
            {
                var delivery = await _deliveryService.CreateAsync(createDeliveryDto);
                return CreatedAtAction(nameof(GetById), new { id = delivery.DeliveryId }, delivery);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "DeliveryPerson,Admin")]
        public async Task<ActionResult<DeliveryDto>> Update(int id, [FromBody] UpdateDeliveryDto updateDeliveryDto)
        {
            try
            {
                var delivery = await _deliveryService.UpdateAsync(id, updateDeliveryDto);
                return Ok(delivery);
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _deliveryService.DeleteAsync(id);
            if (!result)
                return NotFound($"Delivery with id {id} not found");

            return NoContent();
        }
    }
}
