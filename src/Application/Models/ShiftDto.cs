
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ShiftDto
    {
        public int Idshift { get; set; }

        public DateTime Date { get; set; }

        public int Idlocation { get; set; }

        public string Dnitrainer { get; set; }

        public int Peoplelimit { get; set; }


        public static ShiftDto Create(Shift shift)
        {
            return new ShiftDto
            {
                Idshift = shift.Idshift,
                Date = shift.Date,
                Idlocation = shift.Idlocation,
                Dnitrainer = shift.Dnitrainer,
                Peoplelimit = shift.Peoplelimit,
            };

        }
    }
}
