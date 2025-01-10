using StripsDL.Models;

namespace StripsBL.Services
{
    public class UitgeverijenService
    {
        private readonly StripsDbContext _context;

        public UitgeverijenService(StripsDbContext context)
        {
            _context = context;
        }

        public Uitgeverijen? GetUitgeverij(int id)
        {
            return _context.Uitgeverijens.FirstOrDefault(u => u.Id == id);
        }
    }
}
