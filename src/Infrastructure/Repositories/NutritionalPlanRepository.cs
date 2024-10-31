using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class NutritionalPlanRepository : BaseRepository<Nutritionalplan>, INutritionalPlanRepository
    {
        private readonly ApplicationDbContext _context;

        public NutritionalPlanRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

 

        public override List<Nutritionalplan> Get()
        {
            return _context.Set<Nutritionalplan>()
                            .Include(np => np.DniclientNavigation)
                            .Include(np => np.DnitrainerNavigation)
                            .ToList();
        }

        public List<Nutritionalplan> GetByDni(string dni)
        {
            return _context.Set<Nutritionalplan>()
                           .Include(np => np.DniclientNavigation)
                           .Include(np => np.DnitrainerNavigation)
                           .Where(np => np.Dniclient == dni || np.Dnitrainer == dni)
                           .ToList();
        }

        public void Delete(int id)
        {
            var nutritionalPlan = GetById(id);
            if (nutritionalPlan != null)
            {
                _context.Set<Nutritionalplan>().Remove(nutritionalPlan);
                _context.SaveChanges();
            }
        }
    }
}
