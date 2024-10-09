using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly ITrainerRepository _trainerRepository;

        public ShiftService(IShiftRepository shiftRepository, ITrainerRepository trainerRepository)
        {
            _shiftRepository = shiftRepository;
            _trainerRepository = trainerRepository;
        }

        public List<ShiftDto> GetAll()
        {
            var shifts = _shiftRepository.Get();
            return shifts.Select(ShiftDto.Create).ToList();
        }

        public ShiftDto CreateShift(ShiftRequest shiftRequest)
        {
            if (shiftRequest.Dnitrainer != null && _trainerRepository.GetByDni(shiftRequest.Dnitrainer) == null)
            {
                throw new Exception("Trainer not found.");
            }

            var shift = new Shift
            {
                Date = shiftRequest.Date,
                Idlocation = shiftRequest.Idlocation,
                Dnitrainer = shiftRequest.Dnitrainer,
                Peoplelimit = shiftRequest.Peoplelimit
            };

            _shiftRepository.Add(shift);
            return ShiftDto.Create(shift);
        }

        public void UpdateShift(int id, ShiftRequest shiftRequest)
        {
            var shift = _shiftRepository.GetById(id) ?? throw new Exception("Shift not found.");

            if (shiftRequest.Dnitrainer != null && _trainerRepository.GetByDni(shiftRequest.Dnitrainer) == null)
            {
                throw new Exception("Trainer not found.");
            }

            shift.Date = shiftRequest.Date;
            shift.Idlocation = shiftRequest.Idlocation;
            shift.Dnitrainer = shiftRequest.Dnitrainer;
            shift.Peoplelimit = shiftRequest.Peoplelimit;

            _shiftRepository.Update(shift);
        }
    }
}
