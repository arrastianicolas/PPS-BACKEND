using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ShiftRepository : BaseRepository<Shift>, IShiftRepository
    {
        private readonly ApplicationDbContext _context;
        public ShiftRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public Shift? GetShiftsByTrainerAndDate(string dnitrainer, TimeOnly date)
        {
            
            return _context.Shifts
                .FirstOrDefault(s => s.Dnitrainer == dnitrainer && s.Hour == date);
        }

        public Shift? GetShiftByTrainerDayAndHour(string dnitrainer, TimeOnly hour, string dateDay)
        {
            return _context.Shifts
                .FirstOrDefault(s => s.Dnitrainer == dnitrainer
                                     && s.Hour == hour
                                     && s.Dateday == dateDay);
        }

        public List<Shift> GetShiftsByTrainerDni(string trainerDni)
        {
            return _context.Shifts
                .Where(shift => shift.Dnitrainer == trainerDni)
                .ToList();
        }

       }
    }
