using Domain.Interfaces;
using Infrastructure.TempModels;
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
    }
}
