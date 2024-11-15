using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ShiftClientRepository : BaseRepository<Shiftclient>, IShiftClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ShiftClientRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public Shiftclient GetByClientAndDate(string dniClient)
        {
            return _context.Set<Shiftclient>()
                .Include(sc => sc.IdshiftNavigation) 
                .Where(sc => sc.Dniclient == dniClient)
                .FirstOrDefault();
        }

        public int GetLastShiftId(string dniClient)
        {
            return _context.Set<Shiftclient>()
                           .Where(sc => sc.Dniclient == dniClient)
                           .OrderByDescending(sc => sc.Idshift)
                           .Select(sc => sc.Idshift)
                           .FirstOrDefault();
        }

        public void Add(Shiftclient shiftsClient)
        {

            _context.Set<Shiftclient>().Add(shiftsClient);
            _context.SaveChanges(); 
        }
        public void DeleteAll()
        {
            var allShiftClients = _context.Set<Shiftclient>().ToList(); 
            _context.Set<Shiftclient>().RemoveRange(allShiftClients); 
            _context.SaveChanges(); 
        }
        public IEnumerable<Shiftclient> GetShiftsByClientDniForToday(string dniClient)
        {
            var todayDayOfWeek = DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("es-ES"));

            return _context.Shiftclients
                           .Where(sc => sc.Dniclient == dniClient
                                        && sc.IdshiftNavigation.Dateday == todayDayOfWeek) 
                           .Include(sc => sc.IdshiftNavigation) 
                           .ToList();
        }
    }
}
