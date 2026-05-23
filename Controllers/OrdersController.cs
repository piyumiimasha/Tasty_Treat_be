using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly INotificationService _notificationService;

        public OrdersController(IOrderService orderService, IPaymentService paymentService, INotificationService notificationService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound($"Order with id {id} not found");

            return Ok(order);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetByCustomerId(int customerId)
        {
            var orders = await _orderService.GetByCustomerIdAsync(customerId);
            return Ok(orders);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetByStatus(string status)
        {
            var orders = await _orderService.GetByStatusAsync(status);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                var order = await _orderService.CreateAsync(createOrderDto);
                return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> Update(int id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            try
            {
                var order = await _orderService.UpdateAsync(id, updateOrderDto);
                return Ok(order);
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
            var result = await _orderService.DeleteAsync(id);
            if (!result)
                return NotFound($"Order with id {id} not found");

            return NoContent();
        }

        [HttpPost("{id}/approve")]
        public async Task<ActionResult> Approve(int id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                if (order == null) return NotFound($"Order with id {id} not found");

                await _orderService.UpdateAsync(id, new UpdateOrderDto { Status = "In Progress" });

                var payment = await _paymentService.GetByOrderIdAsync(id);
                if (payment != null)
                    await _paymentService.UpdateAsync(payment.PaymentId, new UpdatePaymentDto { PaymentStatus = "Paid" });

                await _notificationService.NotifyUserAsync(
                    order.CustomerId, "OrderStatus",
                    $"Your order #ORD-{id} has been confirmed! We're starting on your cake.", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("{id}/deny")]
        public async Task<ActionResult> Deny(int id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                if (order == null) return NotFound($"Order with id {id} not found");

                await _orderService.UpdateAsync(id, new UpdateOrderDto { Status = "Cancelled" });

                var payment = await _paymentService.GetByOrderIdAsync(id);
                if (payment != null)
                    await _paymentService.UpdateAsync(payment.PaymentId, new UpdatePaymentDto { PaymentStatus = "Failed" });

                await _notificationService.NotifyUserAsync(
                    order.CustomerId, "OrderStatus",
                    $"Your order #ORD-{id} payment could not be verified. Please contact us for assistance.", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
