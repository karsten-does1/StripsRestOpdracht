using Microsoft.EntityFrameworkCore;
using StripsDL.Models;

namespace StripsBL.Services
{
    public class ReeksenService
    {
        private readonly StripsDbContext _context;

        public ReeksenService(StripsDbContext context)
        {
            _context = context;
        }

        public Reeksen? GetReeks(int id)
        {
            return _context.Reeksens
                .Include(r => r.Strips)
                .FirstOrDefault(r => r.Id == id);
        }
    }
}
