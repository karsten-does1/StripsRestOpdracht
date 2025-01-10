using StripsDL.Models;

namespace StripsBL.Services
{
    public class AuteursService
    {
        private readonly StripsDbContext _context;

        public AuteursService(StripsDbContext context)
        {
            _context = context;
        }

        public Auteur? GetAuteur(int id)
        {
            return _context.Auteurs.FirstOrDefault(a => a.Id == id);
        }
    }
}