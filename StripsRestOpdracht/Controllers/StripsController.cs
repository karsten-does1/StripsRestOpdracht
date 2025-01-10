using Microsoft.AspNetCore.Mvc;
using StripsDL.Models;
using StripsBL.Services;
using StripsRest.DTOs;
namespace StripsRest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            if (strip == null)
                return NotFound(new { Message = "Strip not found." });

            var stripDto = new StripDto
            {
                Url = Url.Action(nameof(GetStrip), new { id }),
                Titel = strip.Titel,
                Nr = strip.ReeksNummer ?? 0,
                Reeks = strip.Reeks?.Naam,
                ReeksUrl = strip.Reeks != null ? Url.Action("GetReeks", "Reeksen", new { id = strip.Reeks.Id }) : null,
                Uitgeverij = strip.Uitgeverij.Naam,
                UitgeverijUrl = Url.Action("GetUitgeverij", "Uitgeverijen", new { id = strip.Uitgeverij.Id }),
                Auteurs = strip.Auteurs.Select(a => new AuteurDto
                {
                    Auteur = a.Naam,
                    Url = Url.Action("GetAuteur", "Auteurs", new { id = a.Id })
                }).ToList()
            };

            return Ok(stripDto);
        }

        
        [HttpPost]
        public IActionResult AddStrip([FromBody] Strip strip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid model.", Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
            }

            try
            {
                _stripService.AddStrip(strip);
                return CreatedAtAction(nameof(GetStrip), new { id = strip.Id }, strip);
            }
            catch (InvalidOperationException ex) 
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

    
        [HttpPut("{id}")]
        public IActionResult UpdateStrip(int id, [FromBody] Strip updatedStrip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid model.", Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
            }

            if (id != updatedStrip.Id)
            {
                return BadRequest(new { Message = "The ID in the URL does not match the ID in the body." });
            }

            try
            {
                var existingStrip = _stripService.GetStrip(id);
                if (existingStrip == null)
                {
                    return NotFound(new { Message = "Strip not found." });
                }

                _stripService.UpdateStrip(updatedStrip);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

       
        [HttpDelete("{id}")]
        public IActionResult DeleteStrip(int id)
        {
            try
            {
                var existingStrip = _stripService.GetStrip(id);
                if (existingStrip == null)
                {
                    return NotFound(new { Message = "Strip not found." });
                }

                _stripService.DeleteStrip(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }
}
            