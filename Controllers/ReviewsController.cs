using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll()
        {
            var reviews = await _reviewService.GetAllAsync();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDto>> GetById(int id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null)
                return NotFound($"Review with id {id} not found");

            return Ok(review);
        }

        [HttpGet("item/{itemId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByItemId(int itemId)
        {
            var reviews = await _reviewService.GetByItemIdAsync(itemId);
            return Ok(reviews);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByCustomerId(int customerId)
        {
            var reviews = await _reviewService.GetByCustomerIdAsync(customerId);
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewDto>> Create([FromBody] CreateReviewDto createReviewDto)
        {
            try
            {
                var review = await _reviewService.CreateAsync(createReviewDto);
                return CreatedAtAction(nameof(GetById), new { id = review.ReviewId }, review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReviewDto>> Update(int id, [FromBody] UpdateReviewDto updateReviewDto)
        {
            try
            {
                var review = await _reviewService.UpdateAsync(id, updateReviewDto);
                return Ok(review);
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
            var result = await _reviewService.DeleteAsync(id);
            if (!result)
                return NotFound($"Review with id {id} not found");

            return NoContent();
        }
    }
}
