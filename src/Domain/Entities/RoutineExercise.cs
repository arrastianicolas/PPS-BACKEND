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
        
        [Required]
        [ForeignKey("ExerciseId")]
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }

        [Required]
        [ForeignKey("RoutineId")]
        public int RoutineId { get; set; }
        public Routine Routine { get; set; }

        [Required]
        public string Series {  get; set; }
        [Required]
        public int BreakTime { get; set; }  //Tiempo en segundos
        public int Status { get; set; } //hacer enums

    }
}
