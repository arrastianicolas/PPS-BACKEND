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
        public Shiftclient GetByClientAndDate(string dniClient, DateTime date)
        {

            var dateAsString = date.ToString("yyyy-MM-dd");

            
            return _context.Set<Shiftclient>()
                .Include(sc => sc.IdshiftNavigation) 
                .Where(sc => sc.Dniclient == dniClient && sc.IdshiftNavigation.Dateday == dateAsString)
                .FirstOrDefault();
        }

        public void Add(Shiftclient shiftsClient)
        {

            _context.Set<Shiftclient>().Add(shiftsClient);
            _context.SaveChanges(); // Guardar los cambios en la base de datos
        }
    }
}
