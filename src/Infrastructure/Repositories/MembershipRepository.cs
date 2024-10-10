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
    public class MembershipRepository : BaseRepository<Membership>, IMembershipRepository
    {
        private readonly ApplicationDbContext _context;
        public MembershipRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Membership? GetByMembership(string typeMembership)
        {
            return _context.Set<Membership>().FirstOrDefault(m => m.Type == typeMembership);
        }

        public List<object> GetClientCountByMembership()
        {
            var clientCounts = _context.Clients
                .GroupBy(c => c.Typememberships)  // Agrupa por el tipo de membresía
                .Select(g => new
                {
                    Type = g.Key,            // El tipo de membresía
                    ClientCount = g.Count()   // El número de clientes en ese tipo
                })
                .ToList<object>();  // Convierte a una lista de objetos anónimos

            return clientCounts;
        }

    }
}
