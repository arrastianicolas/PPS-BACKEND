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
        // Nuevo método para obtener todos los clientes con membresía activa
        public IEnumerable<Client> GetAllActiveClients()
        {
            return _context.Set<Client>()
                .Include(c => c.IduserNavigation) // Incluye la relación con User para acceder a sus datos
                .Where(c => c.Isactive == 1) // Filtra solo los clientes activos
                .ToList();
        }
    }
}
