using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class RoutineTrainerRequest
    {

        public int idExercise {  get; set; }
        public TimeOnly Breaktime { get; set; }

        public int Series { get; set; }

        public string Day { get; set; } = null!;
    }
}
