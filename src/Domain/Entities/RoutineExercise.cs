using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RoutineExercise
    {
        [ForeignKey("Exercise")]
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }

        [ForeignKey("Routine")]
        public int TrainerId { get; set; }

        public int ClientId { get; set; }

        public int CorrelativeNumber { get; set; }
        public Routine Routine { get; set; }

        [Required]
        public string Series { get; set; }

        [Required]
        public int BreakTime { get; set; }  // Tiempo en segundos

        public int Status { get; set; } // Usar enums
    }

}
