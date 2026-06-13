using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignRequestsController : ControllerBase
    {
        private readonly IDesignRequestService _service;
        private readonly IBlobStorageService _blobStorageService;
        private readonly string _container;

        public DesignRequestsController(
            IDesignRequestService service,
            IBlobStorageService blobStorageService,
            IConfiguration configuration)
        {
            _service = service;
            _blobStorageService = blobStorageService;
            _container = configuration["AzureBlobStorage:DesignRequestsContainer"] ?? "designerrequests";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DesignRequestDto>>> GetAll()
        {
            var requests = await _service.GetAllAsync();
            return Ok(requests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DesignRequestDto>> GetById(int id)
        {
            var request = await _service.GetByIdAsync(id);
            if (request == null) return NotFound();
            return Ok(request);
        }

        [HttpPost]
        public async Task<ActionResult<DesignRequestDto>> Create(
            [FromForm] CreateDesignRequestDto dto,
            IFormFile? image)
        {
            try
            {
                // Prefer JWT claim over form-submitted value
                var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(claim, out var claimId)) dto.CustomerId = claimId;

                var request = await _service.CreateAsync(dto);

                if (!string.IsNullOrEmpty(dto.ImageUrl))
                {
                    request = await _service.UpdateImageAsync(request.DesignRequestId, dto.ImageUrl);
                }
                else if (image != null && image.Length > 0)
                {
                    try
                    {
                        var imageUrl = await _blobStorageService.UploadImageAsync(image, _container);
                        request = await _service.UpdateImageAsync(request.DesignRequestId, imageUrl);
                    }
                    catch (ArgumentException ex)
                    {
                        await _service.DeleteAsync(request.DesignRequestId);
                        return BadRequest($"Image upload failed: {ex.Message}");
                    }
                }

                return CreatedAtAction(nameof(GetById), new { id = request.DesignRequestId }, request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<DesignRequestDto>> UpdateStatus(int id, [FromBody] UpdateDesignRequestDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Status))
                    return BadRequest("Status is required");

                var request = await _service.UpdateStatusAsync(id, dto.Status, dto.QuotedPrice, dto.AdminMessage);
                return Ok(request);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
