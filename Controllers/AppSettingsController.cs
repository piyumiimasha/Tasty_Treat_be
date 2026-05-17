using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.Data;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AppSettingsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("{key}")]
        public async Task<ActionResult<string>> Get(string key)
        {
            var setting = await _db.AppSettings.FindAsync(key);
            if (setting == null) return NotFound();
            return Ok(setting.Value);
        }

        [HttpPut("{key}")]
        public async Task<ActionResult<string>> Update(string key, [FromBody] string value)
        {
            if (key == "base_price" && (!decimal.TryParse(value, out var price) || price < 0))
                return BadRequest("base_price must be a non-negative number.");

            var setting = await _db.AppSettings.FindAsync(key);
            if (setting == null)
            {
                setting = new AppSetting { Key = key, Value = value };
                _db.AppSettings.Add(setting);
            }
            else
            {
                setting.Value = value;
            }

            await _db.SaveChangesAsync();
            return Ok(setting.Value);
        }
    }
}
