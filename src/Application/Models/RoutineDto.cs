using Infrastructure.TempModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class RoutineDto
    {
        public float Weight { get; set; }

        public float Height { get; set; }

        public string Status { get; set; } 

        public string Description { get; set; }

        public static RoutineDto Create(Routine routine)
        {
            return new RoutineDto
            {
                Weight = routine.Weight,
                Height = routine.Height,
                Status = routine.Status,
                Description = routine.Description,
            };
        }
    }
}
