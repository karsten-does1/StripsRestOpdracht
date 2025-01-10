using Microsoft.EntityFrameworkCore;
using StripsDL.Models;

namespace StripsBL.Services
{
    public class StripService
    {
        private readonly StripsDbContext _context;

        public StripService(StripsDbContext context)
        {
            _context = context;
        }

        public Strip? GetStrip(int id)
        {
            return _context.Strips
                .Include(s => s.Reeks)
                .Include(s => s.Uitgeverij)
                .Include(s => s.Auteurs)
                .FirstOrDefault(s => s.Id == id);
        }

        public void AddStrip(Strip strip)
        {
            _context.Strips.Add(strip);
            _context.SaveChanges();
        }

        public void UpdateStrip(Strip updatedStrip)
        {
            var existingStrip = _context.Strips.FirstOrDefault(s => s.Id == updatedStrip.Id);
            if (existingStrip == null) throw new Exception("Strip not found.");

            existingStrip.Titel = updatedStrip.Titel;
            existingStrip.ReeksId = updatedStrip.ReeksId;
            existingStrip.UitgeverijId = updatedStrip.UitgeverijId;
            existingStrip.ReeksNummer = updatedStrip.ReeksNummer;

            // Update auteurs
            existingStrip.Auteurs.Clear();
            foreach (var auteur in updatedStrip.Auteurs)
            {
                var existingAuteur = _context.Auteurs.FirstOrDefault(a => a.Id == auteur.Id);
                if (existingAuteur != null)
                {
                    existingStrip.Auteurs.Add(existingAuteur);
                }
            }

            _context.SaveChanges();
        }


        public void DeleteStrip(int id)
        {
            var existingStrip = _context.Strips.FirstOrDefault(s => s.Id == id);
            if (existingStrip == null) throw new Exception("Strip not found.");

            _context.Strips.Remove(existingStrip);
            _context.SaveChanges();
        }
    }
}


