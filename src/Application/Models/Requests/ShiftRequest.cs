
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class ShiftRequest
    {


        public string Dateday { get; set; } = null!;

        public TimeOnly Hour { get; set; }

        public int Idlocation { get; set; }

        public string? Dnitrainer { get; set; }

        public int? Peoplelimit { get; set; }

        public int Actualpeople { get; set; }
        public int isActive { get; set; }

    }
}
