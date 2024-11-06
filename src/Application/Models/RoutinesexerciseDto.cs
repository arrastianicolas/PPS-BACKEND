using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class RoutinesexerciseDto
    {
       

        public Exercise exercise { get; set; }

        public TimeOnly Breaktime { get; set; }

        public int Series { get; set; }

        public string Day { get; set; } = null!;
        


        public static RoutinesexerciseDto Create(Routinesexercise routinesexercise, Exercise exercise)
        {
            return new RoutinesexerciseDto
            {
                
                exercise = exercise,
                Breaktime = routinesexercise.Breaktime,
                Series = routinesexercise.Series,
                Day = routinesexercise.Day,
                
            };
        }
    }
}
