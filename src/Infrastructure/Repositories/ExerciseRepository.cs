using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ExerciseRepository : BaseRepository<Exercise>, IExerciseRepository
    {
        private readonly ApplicationDbContext _context;
        public ExerciseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public List<Exercise> GetExercisesByIds(List<int> idExercise)
        {
            if (idExercise == null || !idExercise.Any())
            {
                return new List<Exercise>(); // Retorna una lista vacía si la entrada es nula o vacía
            }

            // Consulta los ejercicios que coinciden con los IDs proporcionados
            return _context.Exercises
                .Where(exercise => idExercise.Contains(exercise.Idexercise))
                .ToList(); // Ejecuta la consulta y devuelve la lista de ejercicios
        }
        

    }
}
