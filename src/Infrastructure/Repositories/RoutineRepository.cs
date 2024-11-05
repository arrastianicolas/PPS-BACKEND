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
    public class RoutineRepository : BaseRepository<Routine>, IRoutineRepository
    {
        private readonly ApplicationDbContext _context;
        public RoutineRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public override List<Routine> Get()
        {
            return _context.Set<Routine>()
                            .Include(np => np.DniclientNavigation)
                            .Include(np => np.DnitrainerNavigation)
                            .ToList();
        }

        public List<Routine> GetByDni(string dni)
        {
            return _context.Set<Routine>()
                           .Include(np => np.DniclientNavigation)
                           .Include(np => np.DnitrainerNavigation)
                           .Where(np => np.Dniclient == dni || np.Dnitrainer == dni)
                           .ToList();
        }

    }
}
