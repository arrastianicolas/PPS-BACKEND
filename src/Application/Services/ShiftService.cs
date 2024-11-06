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
        private readonly IMailService _mailService;
        public ShiftService(IShiftRepository shiftRepository, ITrainerRepository trainerRepository, ILocationRepository locationRepository, IShiftClientRepository shiftClientRepository, IUserRepository userRepository, IClientRepository clientRepository, IMailService mailService)
        {
            _shiftRepository = shiftRepository;
            _trainerRepository = trainerRepository;
            _locationRepository = locationRepository;
            _shiftClientRepository = shiftClientRepository;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _mailService = mailService;
        }

        public List<ShiftDto> GetAll()
        {
            var shifts = _shiftRepository.Get();
            return shifts.Select(ShiftDto.Create).ToList();
        }
        public ShiftMydetailsDto GetMyShiftDetails(int Iduser)
        {
            var user = _userRepository.GetById(Iduser);
            if (user == null)
            {
                throw new NotFoundException("Usuario no encontrado");
            }

            var clientUser = _clientRepository.GetClientByUserId(Iduser);
            if (clientUser == null)
            {
                throw new NotFoundException("No se encontró al cliente.");
            }

            // Obtener los turnos del cliente para el día de hoy
            var shifts = _shiftClientRepository.GetShiftsByClientDniForToday(clientUser.Dniclient);


            var shift = shifts.FirstOrDefault()?.IdshiftNavigation;
            var trainer = _trainerRepository.GetByDni(shift.Dnitrainer);
            if (trainer == null)
            {
                throw new NotFoundException("No se encontraron trainers.");
            }
            var location = _locationRepository.GetById(shift.Idlocation);
            if (shift == null)
            {
                throw new NotFoundException("No se encontraron turnos para el cliente en el día de hoy.");
            }


            return new ShiftMydetailsDto
            {
                Idshift = shift.Idshift,
                Dateday = shift.Dateday,
                Hour = shift.Hour,
                Idlocation = shift.Idlocation,
                Peoplelimit = shift.Peoplelimit,
                Actualpeople = shift.Actualpeople,
                Isactive = shift.IsActive,
                Dnitrainer = shift.Dnitrainer,
                Firstname = trainer.Firstname,
                Lastname = trainer.Lastname,
                Adress = location.Adress,
                Namelocation = location.Name,
            };
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

            var client = _clientRepository.GetClientByUserId(Iduser);
            // Obtener el turno por ID
            var shift = _shiftRepository.GetById(shiftId);
            if (shift == null)
            {
                throw new Exception("No se encontró el turno.");
            }



            // Obtiene la hora actual como un TimeOnly
            var now = TimeOnly.FromDateTime(DateTime.Now);

            // Validar que la fecha del turno no sea en el pasado
            if (shift.Hour < now)
            {
                throw new Exception("No se puede reservar un turno en el pasado.");
            }


            // Validar que el cliente no haya reservado otro turno en el mismo día
            var existingReservation = _shiftClientRepository
                .GetByClientAndDate(client.Dniclient);
            if (existingReservation != null)
            {
                throw new Exception("Ya has reservado un turno para este día.");
            }

            // Registrar la reserva
            shift.Actualpeople = shift.Actualpeople ?? 0;
            shift.Actualpeople++;
            _shiftRepository.Update(shift);

            var shiftClient = new Shiftclient
            {
                Dniclient = client.Dniclient,
                Idshift = shiftId
            };
            var location = _locationRepository.GetById(shift.Idlocation);
            _shiftClientRepository.Add(shiftClient);
            _mailService.Send(
                  $"Nuevo turno reservado en Training Center!",
                  $"Hola {client.Firstname}, usted ha reservado un turno para hoy a las {shift.Hour}hs.\n Lo esperamos en la {location.Name} ubicada en {location.Adress}!",
                  user.Email ?? throw new Exception("Trainer email not found")
              );
        }

        public List<ShiftDto> AssignTrainerToShifts(AssignTrainerRequest request)
        {
            var assignedShifts = new List<ShiftDto>();
            var trainer = _trainerRepository.GetByDni(request.Dnitrainer);

            foreach (var shiftId in request.ShiftIds)
            {
                // Buscar el turno por su ID
                var shift = _shiftRepository.GetById(shiftId);
                if (shift == null)
                {
                    throw new Exception($"Shift with ID {shiftId} not found.");
                }

                // Verificar si ya tiene un dnitrainer asignado
                if (shift.Dnitrainer != null)
                {
                    throw new Exception($"Shift with ID {shiftId} already has a trainer assigned.");
                }

                // Validar que el dnitrainer no esté asignado a otro turno en el mismo día y a la misma hora
                var overlappingShift = _shiftRepository.GetShiftByTrainerDayAndHour(trainer.Dnitrainer, shift.Hour, shift.Dateday);
                if (overlappingShift != null)
                {
                    throw new Exception($"Trainer {trainer.Dnitrainer} is already assigned to another shift at {shift.Hour} on {shift.Dateday}.");
                }


                // Asignar el nuevo trainer
                shift.Dnitrainer = request.Dnitrainer;
                _shiftRepository.Update(shift);  // Actualizar el turno con el nuevo trainer
                assignedShifts.Add(ShiftDto.Create(shift));
            }
            var user = _userRepository.GetById(trainer.Iduser);
            // Consolidar los turnos asignados en un solo correo
            if (assignedShifts.Any())
            {
                var shiftsInfo = string.Join(", ", assignedShifts.Select(s => $"{s.Dateday} a las {s.Hour}"));
                _mailService.Send(
                    $"Usted Tiene una nueva asignacion {trainer.Firstname} {trainer.Lastname} del Training Center",
                    $"Hola {trainer.Firstname}, usted fue asignado a los turnos en las siguientes fechas y horarios: {shiftsInfo}.",
                    user.Email ?? throw new Exception("Trainer email not found")
                );
            }

            return assignedShifts;  // Devolver los turnos asignados actualizados
        }

        public List<ShiftDto> GetShiftsByLocationAndDate(ShiftLocationDayRequest request)
        {
            // Obtener todos los turnos
            var shifts = _shiftRepository.Get();

            // Filtrar los turnos por la ubicación y la fecha
            var filteredShifts = shifts
                .Where(s => s.Idlocation == request.locationId && s.Dateday == request.day)
                .Select(ShiftDto.Create)
                .ToList();

            if (!filteredShifts.Any())
            {
                throw new NotFoundException("No shifts found for the specified location and date.");
            }

            return filteredShifts;
        }
        public ShiftMydetailsDto GetNextTrainerShift(int idUser)
        {
            var user = _userRepository.GetById(idUser);
            if (user == null)
            {
                throw new NotFoundException("Usuario no encontrado.");
            }

            // Obtener el entrenador asociado al ID del usuario
            var trainer = _trainerRepository.GetTrainerByUserId(idUser);
            if (trainer == null)
            {
                throw new NotFoundException("No se encontró al entrenador.");
            }

            // Obtener todos los turnos del entrenador
            var shifts = _shiftRepository.GetShiftsByTrainerDni(trainer.Dnitrainer);

            // Obtener la fecha y hora actual
            DateTime currentDateTime = DateTime.Now;

            // Filtrar y encontrar el siguiente turno válido
            var nextShift = shifts
                .Select(shift => new
                {
                    Shift = shift,
                    // Convertir Dateday y Hour a DateTime
                    ShiftDateTime = TryGetNextDateTime(shift.Dateday, shift.Hour)
                })
                .Where(s => s.ShiftDateTime.HasValue && s.ShiftDateTime.Value >= currentDateTime) // Asegurarse que sea un futuro
                .OrderBy(s => s.ShiftDateTime)
                .Select(s => s.Shift)
                .FirstOrDefault();

            if (nextShift == null)
            {
                throw new NotFoundException("No se encontraron turnos futuros.");
            }

            var location = _locationRepository.GetById(nextShift.Idlocation);
            if (location == null)
            {
                throw new NotFoundException("No se encontró la ubicación del turno.");
            }

            return new ShiftMydetailsDto
            {
                Idshift = nextShift.Idshift,
                Dateday = nextShift.Dateday,
                Hour = nextShift.Hour, // Asegúrate de convertir TimeOnly a string si es necesario
                Idlocation = nextShift.Idlocation,
                Peoplelimit = nextShift.Peoplelimit,
                Actualpeople = nextShift.Actualpeople,
                Isactive = nextShift.IsActive,
                Dnitrainer = nextShift.Dnitrainer,
                Firstname = trainer.Firstname,
                Lastname = trainer.Lastname,
                Adress = location.Adress,
                Namelocation = location.Name,
            };
        }

        public void UnassignTrainerFromShift(int shiftId)
        {
            var shift = _shiftRepository.GetById(shiftId);
            if (shift == null)
            {
                throw new Exception($"Shift with ID {shiftId} not found.");
            }

            if (shift.Dnitrainer == null)
            {
                throw new Exception($"Shift with ID {shiftId} does not have a trainer assigned.");
            }

            var trainerDni = shift.Dnitrainer;  
            shift.Dnitrainer = null;
            _shiftRepository.Update(shift); 

            var trainer = _trainerRepository.GetByDni(trainerDni);
            if (trainer != null)
            {
                var user = _userRepository.GetById(trainer.Iduser);
                if (user?.Email != null)
                {
                    _mailService.Send(
                        "Has sido desasignado de un turno",
                        $"Hola {trainer.Firstname}, has sido desasignado del turno el {shift.Dateday} a las {shift.Hour}.",
                        user.Email
                    );

                }
            }
        }

        private DateTime? TryGetNextDateTime(string dayName, TimeOnly time)
        {
            // Convertir el nombre del día a DayOfWeek
            DayOfWeek targetDay;
            switch (dayName.ToLower())
            {
                case "lunes":
                    targetDay = DayOfWeek.Monday;
                    break;
                case "martes":
                    targetDay = DayOfWeek.Tuesday;
                    break;
                case "miércoles":
                    targetDay = DayOfWeek.Wednesday;
                    break;
                case "jueves":
                    targetDay = DayOfWeek.Thursday;
                    break;
                case "viernes":
                    targetDay = DayOfWeek.Friday;
                    break;
                case "sábado":
                    targetDay = DayOfWeek.Saturday;
                    break;
                case "domingo":
                    targetDay = DayOfWeek.Sunday;
                    break;
                default:
                    return null; // Día no válido
            }

            // Calcular la fecha del próximo día
            DateTime today = DateTime.Today;
            int daysUntilNext = ((int)targetDay - (int)today.DayOfWeek + 7) % 7;
            if (daysUntilNext == 0 && DateTime.Now.TimeOfDay >= time.ToTimeSpan()) // Si es hoy y ya pasó la hora, buscar para el próximo
            {
                daysUntilNext = 7; // Ir a la próxima semana
            }

            DateTime nextDate = today.AddDays(daysUntilNext);
            return nextDate.Add(time.ToTimeSpan()); // Combina fecha y hora
        }

    }
}