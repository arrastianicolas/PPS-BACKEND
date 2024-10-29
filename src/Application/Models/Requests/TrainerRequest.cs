using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class TrainerRequest
    {
        public string Dnitrainer { get; set; } = null!;

        public DateOnly Birthdate { get; set; }

        public string Phonenumber { get; set; } = null!;

        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public int Isactive { get; set; }
    }
}
