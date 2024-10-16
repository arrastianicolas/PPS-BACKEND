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
    public class TrainerRepository : BaseRepository<Trainer> , ITrainerRepository 
    {
        private readonly ApplicationDbContext _context;
        public TrainerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Trainer? GetTrainerByUserId(int userId)
        {
            return _context.Set<Trainer>().FirstOrDefault(c => c.Iduser == userId);
        }
        public Trainer? GetByDni(string dni)
        {
            return _context.Set<Trainer>().FirstOrDefault(c => c.Dnitrainer == dni);
        }
    }
}
