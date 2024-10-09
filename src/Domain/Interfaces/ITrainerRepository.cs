using Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITrainerRepository : IBaseRepository<Trainer>
    {
        Trainer? GetTrainerByUserId(int userId);
        Trainer? GetByDni(string dni);
    }
}
