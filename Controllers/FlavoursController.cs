using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/Flavours")]
    [ApiController]
    public class FlavoursController : ControllerBase
    {
        private readonly IItemFlavourService _service;

        public FlavoursController(IItemFlavourService service)
        {
            _service = service;
        }

        [HttpGet("distinct")]
        public async Task<ActionResult<IEnumerable<string>>> GetDistinct()
        {
            var names = await _service.GetDistinctNamesAsync();
            return Ok(names);
        }
    }
}
