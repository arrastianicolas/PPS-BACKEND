using Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class RoutineDto
    {
        public string Weight { get; set; }

        public string Height { get; set; }

        public string Status { get; set; } 

        public string Description { get; set; }
        public int Days {  get; set; }
        public string? ClientName { get; set; }
        public string? ClientBirthdate { get; set; }

        public static RoutineDto Create(Routine routine, string? clientName = null, string? clientBirthdate = null)
        {
            return new RoutineDto
            {
                Weight = routine.Weight,
                Height = routine.Height,
                Status = routine.Status,
                Description = routine.Description,
                Days = routine.Days,
                ClientName = clientName,
                ClientBirthdate = clientBirthdate
            };
        }
    }
}
