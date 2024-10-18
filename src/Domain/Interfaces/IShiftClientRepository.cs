using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IShiftClientRepository : IBaseRepository<Shiftclient>
    {
        Shiftclient GetByClientAndDate(string dniClient, DateTime date);
        void Add(Shiftclient shiftsClient);
    }
}
