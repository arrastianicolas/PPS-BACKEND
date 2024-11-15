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
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        private readonly ApplicationDbContext _context;
        public ClientRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Client? GetClientByUserId(int userId)
        {
            return _context.Set<Client>().FirstOrDefault(c => c.Iduser == userId);
        }
        public Client? GetByDni (string dni)
        {
            return _context.Set<Client>().FirstOrDefault(c => c.Dniclient == dni);
        }
        
        public IEnumerable<Client> GetAllActiveClients()
        {
            return _context.Set<Client>()
                .Include(c => c.IduserNavigation) 
                .Where(c => c.Isactive == 1) 
                .ToList();
        }
        public IEnumerable<object> GetNewClientsCountPerMonth()
        {
            return _context.Set<Client>()
                .GroupBy(c => new { c.Startdatemembership.Year, c.Startdatemembership.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToList();
        }
    }
}
