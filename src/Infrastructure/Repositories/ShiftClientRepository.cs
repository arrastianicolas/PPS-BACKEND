using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Add(Shiftclient shiftsClient)
        {

            _context.Set<Shiftclient>().Add(shiftsClient);
            _context.SaveChanges(); // Guardar los cambios en la base de datos
        }
        public void DeleteAll()
        {
            var allShiftClients = _context.Set<Shiftclient>().ToList(); // Obtener todos los registros
            _context.Set<Shiftclient>().RemoveRange(allShiftClients); // Eliminar todos
            _context.SaveChanges(); // Guardar cambios
        }
        public IEnumerable<Shiftclient> GetShiftsByClientDniForToday(string dniClient)
        {
            var todayDayOfWeek = DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("es-ES"));

            return _context.Shiftclients
                           .Where(sc => sc.Dniclient == dniClient
                                        && sc.IdshiftNavigation.Dateday == todayDayOfWeek) // Filtrar por día actual
                           .Include(sc => sc.IdshiftNavigation) // Incluye los detalles del turno (Shift)
                           .ToList();
        }
    }
}
