using Microsoft.AspNetCore.Mvc;
using StripsBL.Services;

namespace StripsRest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuteursController : ControllerBase
    {
        private readonly AuteursService _auteursService;

        public AuteursController(AuteursService auteursService)
        {
            _auteursService = auteursService;
        }

        [HttpGet("{id}")]
        public IActionResult GetAuteur(int id)
        {
            var auteur = _auteursService.GetAuteur(id);
            if (auteur == null)
                return NotFound(new { Message = "Auteur not found." });

            return Ok(new
            {
                Naam = auteur.Naam,
                Email = auteur.Email,
                Url = Url.Action(nameof(GetAuteur), new { id })
            });
        }
    }
}