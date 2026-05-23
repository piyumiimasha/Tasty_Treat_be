using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IConfiguration _configuration;

        public PaymentsController(IPaymentService paymentService, IBlobStorageService blobStorageService, IConfiguration configuration)
        {
            _paymentService = paymentService;
            _blobStorageService = blobStorageService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetAll()
        {
            var payments = await _paymentService.GetAllAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetById(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
                return NotFound($"Payment with id {id} not found");

            return Ok(payment);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<PaymentDto>> GetByOrderId(int orderId)
        {
            var payment = await _paymentService.GetByOrderIdAsync(orderId);
            if (payment == null)
                return NotFound($"Payment for order {orderId} not found");

            return Ok(payment);
        }

        [HttpGet("transaction/{transactionId}")]
        public async Task<ActionResult<PaymentDto>> GetByTransactionId(string transactionId)
        {
            var payment = await _paymentService.GetByTransactionIdAsync(transactionId);
            if (payment == null)
                return NotFound($"Payment with transaction id {transactionId} not found");

            return Ok(payment);
        }

        [HttpPost("upload-slip")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<object>> UploadSlip(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided");
            if (!file.ContentType.StartsWith("image/"))
                return BadRequest("Only image files are allowed");
            if (file.Length > 5 * 1024 * 1024)
                return BadRequest("File too large (max 5 MB)");

            try
            {
                var container = _configuration["AzureBlobStorage:PaymentSlipsContainer"] ?? "paymentslips";
                var url = await _blobStorageService.UploadImageAsync(file, container);
                return Ok(new { url });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDto>> Create([FromBody] CreatePaymentDto createPaymentDto)
        {
            try
            {
                var payment = await _paymentService.CreateAsync(createPaymentDto);
                return CreatedAtAction(nameof(GetById), new { id = payment.PaymentId }, payment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentDto>> Update(int id, [FromBody] UpdatePaymentDto updatePaymentDto)
        {
            try
            {
                var payment = await _paymentService.UpdateAsync(id, updatePaymentDto);
                return Ok(payment);
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
            var result = await _paymentService.DeleteAsync(id);
            if (!result)
                return NotFound($"Payment with id {id} not found");

            return NoContent();
        }
    }
}
