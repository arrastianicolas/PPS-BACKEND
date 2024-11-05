using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ExerciseDto
    {
        public int Idexercise { get; set; }

        public string Name { get; set; }

        public string Musclegroup { get; set; }

        public static ExerciseDto Create(Exercise exercise)
        {
            return new ExerciseDto
            {
                Idexercise = exercise.Idexercise,
                Name = exercise.Name,
                Musclegroup = exercise.Musclegroup
            };

        }
    }
}
