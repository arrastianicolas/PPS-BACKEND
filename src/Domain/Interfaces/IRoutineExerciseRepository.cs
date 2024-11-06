
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IRoutineExerciseRepository : IBaseRepository<Routinesexercise>
    {
        List<Routinesexercise> GetByDni(string dniClient);
    }
}
