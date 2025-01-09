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

        public void AddStrip(Strip strip)
        {
            if (strip.Auteurs.Distinct().Count() != strip.Auteurs.Count)
                throw new ArgumentException("Een strip mag geen dubbele auteurs hebben.");

            if (strip.Reeks != null && _context.Strips.Any(s => s.Reeks == strip.Reeks && s.ReeksNummer == strip.ReeksNummer))
                throw new ArgumentException("Een reeks mag geen dubbele strips bevatten.");

            _context.Strips.Add(strip);
            _context.SaveChanges();
        }

        public Strip GetStrip(int id) => _context.Strips
            .Include(s => s.Auteurs)
            .Include(s => s.Uitgeverij)
            .Include(s => s.Reeks)
            .FirstOrDefault(s => s.Id == id);
    }
}


