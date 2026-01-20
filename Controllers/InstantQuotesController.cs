using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstantQuotesController : ControllerBase
    {
        private readonly IInstantQuoteService _instantQuoteService;

        public InstantQuotesController(IInstantQuoteService instantQuoteService)
        {
            _instantQuoteService = instantQuoteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstantQuoteDto>>> GetAll()
        {
            var quotes = await _instantQuoteService.GetAllAsync();
            return Ok(quotes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstantQuoteDto>> GetById(int id)
        {
            var quote = await _instantQuoteService.GetByIdAsync(id);
            if (quote == null)
                return NotFound($"InstantQuote with id {id} not found");

            return Ok(quote);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<InstantQuoteDto>>> GetByCustomerId(int customerId)
        {
            var quotes = await _instantQuoteService.GetByCustomerIdAsync(customerId);
            return Ok(quotes);
        }

        [HttpPost]
        public async Task<ActionResult<InstantQuoteDto>> Create([FromBody] CreateInstantQuoteDto createInstantQuoteDto)
        {
            try
            {
                var quote = await _instantQuoteService.CreateAsync(createInstantQuoteDto);
                return CreatedAtAction(nameof(GetById), new { id = quote.QuoteId }, quote);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InstantQuoteDto>> Update(int id, [FromBody] UpdateInstantQuoteDto updateInstantQuoteDto)
        {
            try
            {
                var quote = await _instantQuoteService.UpdateAsync(id, updateInstantQuoteDto);
                return Ok(quote);
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
            var result = await _instantQuoteService.DeleteAsync(id);
            if (!result)
                return NotFound($"InstantQuote with id {id} not found");

            return NoContent();
        }
    }
}
