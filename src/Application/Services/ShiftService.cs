using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Exceptions;
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
        private readonly IShiftClientRepository _shiftClientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        public ShiftService(IShiftRepository shiftRepository, ITrainerRepository trainerRepository, ILocationRepository locationRepository, IShiftClientRepository shiftClientRepository, IUserRepository userRepository, IClientRepository clientRepository)
        {
            _shiftRepository = shiftRepository;
            _trainerRepository = trainerRepository;
            _locationRepository = locationRepository;
            _shiftClientRepository = shiftClientRepository;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
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
                Actualpeople = 0,
                IsActive = 1,
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
            shift.Actualpeople = shiftRequest.Actualpeople;
            shift.IsActive = shiftRequest.isActive;

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
        public void ReserveShift(int shiftId, int Iduser)
        {
            //Para sacar de las claims al client
            var user = _userRepository.GetById(Iduser);
            if (user == null)
            {
                throw new NotFoundException("Usuario no encontrado");
            }

            var dniclient = _clientRepository.GetClientByUserId(Iduser);
            // Obtener el turno por ID
            var shift = _shiftRepository.GetById(shiftId);
            if (shift == null)
            {
                throw new Exception("No se encontró el turno.");
            }

            // Convertir el Dateday (string) a DateTime para la validación
            if (!DateTime.TryParse(shift.Dateday, out DateTime shiftDate))
            {
                throw new Exception("La fecha del turno no es válida.");
            }

            // Validar que la fecha del turno no sea en el pasado
            if (shiftDate < DateTime.Now.Date)
            {
                throw new Exception("No se puede reservar un turno en el pasado.");
            }

            // Validar que el cliente no haya reservado otro turno en el mismo día
            var existingReservation = _shiftClientRepository
                .GetByClientAndDate(dniclient.Dniclient, shiftDate);
            if (existingReservation != null)
            {
                throw new Exception("Ya has reservado un turno para este día.");
            }

            // Registrar la reserva
            var shiftClient = new Shiftclient
            {
                Dniclient = dniclient.Dniclient,
                Idshift = shiftId
            };

            _shiftClientRepository.Add(shiftClient);
        }

    }
}

