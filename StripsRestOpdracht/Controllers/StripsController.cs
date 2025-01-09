using Microsoft.AspNetCore.Mvc;
using StripsDL.Models;
using StripsBL.Services;
namespace StripsRest.Controllers
{
    public class StripsController : ControllerBase
    {
        private readonly StripService _stripService;

        public StripsController(StripService stripService)
        {
            _stripService = stripService;
        }

        [HttpGet("{id}")]
        public IActionResult GetStrip(int id)
        {
            var strip = _stripService.GetStrip(id);
            if (strip == null) return NotFound("Strip not found.");
            return Ok(strip);
        }

        [HttpPost]
        public IActionResult AddStrip([FromBody] Strip strip)
        {
            try
            {
                _stripService.AddStrip(strip);
                return CreatedAtAction(nameof(GetStrip), new { id = strip.Id }, strip);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

