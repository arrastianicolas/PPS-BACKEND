using Domain.Entities;
using Domain.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RoutineRepository : BaseRepository<Routine>, IRoutineRepository
    {
        private readonly ApplicationDbContext _context;
        public RoutineRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public int GetNextCorrelativenumber(string dniClient)
        {
            var lastRoutine = _context.Routines
                .Where(r => r.Dniclient == dniClient.ToString()) // Cambia esto si es necesario
                .OrderByDescending(r => r.Correlativenumber)
                .FirstOrDefault();

            // Si no hay rutinas, el siguiente número es 1, de lo contrario, suma 1 al último número
            return lastRoutine?.Correlativenumber + 1 ?? 1;
        }
    }
}
