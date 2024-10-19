using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LocationRepository: BaseRepository<Location>, ILocationRepository
    {
        private readonly ApplicationDbContext _context;
        public LocationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Location> GetActives()
        {
            return _context.Set<Location>().Include(l => l.Shifts).Where(l => l.Isactive == 1).ToList();
        }

        public override List<Location>? Get()
        {
            return _context.Set<Location>().Include(l => l.Shifts).ToList();
        }

        public Location? GetById(int id)
        {
            return _context.Set<Location>().Include(l => l.Shifts).FirstOrDefault(l => l.Idlocation == id);

        }

        public async Task<List<object>> GetClientsCountByLocationAsync()
        {
            var clientsCountByLocation = await _context.Locations
                .Where(l => l.Isactive == 1) // Solo locations activas
                .Select(l => new
                {
                    LocationName = l.Name,
                    ClientsCount = l.Shifts
                        .Count(s => s.Shiftclient != null) // Contamos solo si Shiftclient no es nulo
                })
                .ToListAsync();

            // Devolver como lista de objetos anónimos
            return clientsCountByLocation.Cast<object>().ToList();
        }
    }
}
