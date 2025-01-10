using Microsoft.AspNetCore.Mvc;
using StripsBL.Services;
using StripsDL.Models;
using StripsRest.DTOs;

namespace StripsRest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReeksenController : ControllerBase
    {
        private readonly ReeksenService _reeksService;

        public ReeksenController(ReeksenService reeksService)
        {
            _reeksService = reeksService;
        }

        [HttpGet("{id}")]
        public IActionResult GetReeks(int id)
        {
            var reeks = _reeksService.GetReeks(id);
            if (reeks == null)
            {
                return NotFound(new { Message = "Reeks not found." });
            }

            var reeksDto = new ReeksenDto
            {
                Naam = reeks.Naam,
                Url = Url.Action(nameof(GetReeks), new { id }),
                Strips = reeks.Strips.Select(s => new StripDto
                {
                    Nr = s.ReeksNummer ?? 0,
                    Titel = s.Titel,
                    Url = Url.Action("GetStrip", "Strips", new { id = s.Id })
                }).ToList()
            };

            return Ok(reeksDto);
        }
    }
}