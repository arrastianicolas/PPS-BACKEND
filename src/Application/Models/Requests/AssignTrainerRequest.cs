using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class AssignTrainerRequest
    {
        public List<int> ShiftIds { get; set; } = new List<int>();
        public string Dnitrainer { get; set; } = string.Empty;

    }
}
