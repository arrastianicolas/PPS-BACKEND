using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        Client? GetClientByUserId(int userId);
        Client? GetByDni(string dni);
    }
}
