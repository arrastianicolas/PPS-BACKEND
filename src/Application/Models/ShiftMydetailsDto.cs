using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ShiftMydetailsDto
    {
        public int Idshift { get; set; }
        public string Dateday { get; set; } = null!;
        public TimeOnly Hour { get; set; }
        public int Idlocation { get; set; }
        public int? Peoplelimit { get; set; }
        public int? Actualpeople { get; set; }
        public int Isactive { get; set; }

        // Detalles del Trainer
        public string? Dnitrainer { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        //detalles de location
        public string Adress { get; set; } = null!;

        public string Namelocation { get; set; } = null!;
    }
}
