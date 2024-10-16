﻿using Application.Interfaces;
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
        private readonly ILocationRepository _locationRepository;

        public ShiftService(IShiftRepository shiftRepository, ITrainerRepository trainerRepository, ILocationRepository locationRepository)
        {
            _shiftRepository = shiftRepository;
            _trainerRepository = trainerRepository;
            _locationRepository = locationRepository;
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

            var location = _locationRepository.GetById(shiftRequest.Idlocation) ?? throw new Exception("Location not found.");

            if (location.Isactive == 0)
            {
                throw new Exception("Location is inactive. Cannot create shift.");
            }

            var shift = new Shift
            {
                Dateday = shiftRequest.Dateday,
                Hour = shiftRequest.Hour,
                Idlocation = shiftRequest.Idlocation,
                Dnitrainer = shiftRequest.Dnitrainer,
                Peoplelimit = shiftRequest.Peoplelimit,
                Totaldays = shiftRequest.Totaldays,
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

            var location = _locationRepository.GetById(shiftRequest.Idlocation) ?? throw new Exception("Location not found.");

            if (location.Isactive == 0)
            {
                throw new Exception("Location is inactive. Cannot update shift.");
            }


            shift.Dateday = shiftRequest.Dateday;
            shift.Hour = shiftRequest.Hour;
            shift.Idlocation = shiftRequest.Idlocation;
            shift.Dnitrainer = shiftRequest.Dnitrainer;
            shift.Peoplelimit = shiftRequest.Peoplelimit;
            shift.Totaldays = shiftRequest.Totaldays;

            _shiftRepository.Update(shift);
        }

        public void AddShift(int shiftId, int locationId)
        {
            var shift = _shiftRepository.GetById(shiftId) ?? throw new Exception("Shift not found.");
            var location = _locationRepository.GetById(locationId) ?? throw new Exception("Location not found.");

            if (location.Isactive == 0)
            {
                throw new Exception("Location is inactive. Cannot add shift.");
            }

            location.Shifts.Add(shift);
            _locationRepository.Update(location);
        }

        //public void RemoveShift(int shiftId, int locationId)
        //{
        //    var location = _locationRepository.GetById(locationId) ?? throw new Exception("Location not found.");
        //    var shift = location.Shifts.FirstOrDefault(s => s.Idshift == shiftId) ?? throw new Exception("Shift not found.");

        //    location.Shifts.Remove(shift);
        //    _locationRepository.Update(location);
        //}

    }
}

