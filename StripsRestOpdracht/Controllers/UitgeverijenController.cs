using Microsoft.AspNetCore.Mvc;
using StripsBL.Services;

namespace StripsRest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UitgeverijenController : ControllerBase
    {
        private readonly UitgeverijenService _uitgeverijenService;

        public UitgeverijenController(UitgeverijenService uitgeverijenService)
        {
            _uitgeverijenService = uitgeverijenService;
        }

        [HttpGet("{id}")]
        public IActionResult GetUitgeverij(int id)
        {
            var uitgeverij = _uitgeverijenService.GetUitgeverij(id);
            if (uitgeverij == null)
                return NotFound(new { Message = "Uitgeverij not found." });

            return Ok(new
            {
                Naam = uitgeverij.Naam,
                Adres = uitgeverij.Adres,
                Url = Url.Action(nameof(GetUitgeverij), new { id })
            });
        }
    }
}
