using Application.Models;
using Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IShiftService
    {
        List<ShiftDto> GetAll();
        ShiftDto CreateShift(ShiftRequest shiftRequest);
        void UpdateShift(int id, ShiftRequest shiftRequest);
    }
}
