using Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IShiftRepository : IBaseRepository<Shift>
    {
        Shift? GetShiftsByTrainerAndDate(string dnitrainer, TimeOnly date);
        List<Shift> GetShiftsByTrainerDni(string trainerDni);
    }
}
