
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;


namespace Infrastructure.Repositories
{
    public class RoutineExerciseRepository : BaseRepository<Routinesexercise>, IRoutineExerciseRepository
    {
        private readonly ApplicationDbContext _context;
        public RoutineExerciseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public List<Routinesexercise> GetByDni(string dni)
        {
            return _context.Set<Routinesexercise>()
                .Include(re => re.IdroutineNavigation)
                .Include(re => re.IdexerciseNavigation)
                .Where(re => re.IdroutineNavigation.Dniclient == dni ||
                             re.IdroutineNavigation.Dnitrainer == dni)
                .Select(re => new Routinesexercise
                {
                  
                    IdexerciseNavigation = re.IdexerciseNavigation != null ? new Exercise
                    {
                        Idexercise = re.IdexerciseNavigation.Idexercise,
                        Name = re.IdexerciseNavigation.Name,
                        Musclegroup = re.IdexerciseNavigation.Musclegroup
                    } : null,
                    Breaktime = re.Breaktime,
                    Series = re.Series,
                    Day = re.Day
                })
                .ToList();

        }
       


    }

}
