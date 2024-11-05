
using Application.Models.Requests;
using Domain.Entities;
using Domain.Interfaces;


namespace Infrastructure.Repositories
{
    public class RoutineExerciseRepository : BaseRepository<Routinesexercise>, IRoutineExerciseRepository
    {
        private readonly ApplicationDbContext _context;
        public RoutineExerciseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
