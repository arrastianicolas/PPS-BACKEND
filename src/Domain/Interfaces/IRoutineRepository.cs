using Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRoutineRepository : IBaseRepository<Routine>
    {
         List<Routine> GetByDni(string dni);
        Routine ChangeStatusToDone(string dniClient);
    }
}
