using Application.Interfaces;
using Application.Models;
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
    public class RoutinesexerciseService : IRoutinesexerciseService
    {
        private readonly IRoutineRepository _routineRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IShiftClientRepository _shiftClientRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly ITrainerRepository _trainerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IRoutineExerciseRepository _routineExerciseRepository;
        private readonly IExerciseRepository _exerciseRepository;
        public RoutinesexerciseService(IClientRepository clientRepository,
            IRoutineRepository routineRepository,
            IShiftClientRepository shiftClientRepository,
            IShiftRepository shiftRepository,
            ITrainerRepository trainerRepository,
            IUserRepository userRepository,
            IMailService mailService,
            IRoutineExerciseRepository routineExerciseRepository,
            IExerciseRepository exerciseRepository)
        {
            _routineRepository = routineRepository;
            _clientRepository = clientRepository;
            _shiftClientRepository = shiftClientRepository;
            _shiftRepository = shiftRepository;
            _trainerRepository = trainerRepository;
            _userRepository = userRepository;
            _mailService = mailService;
            _routineExerciseRepository = routineExerciseRepository;
            _exerciseRepository = exerciseRepository;

        }
        public object GetRoutineWithExercises(int userId)
        {
            var client = _clientRepository.GetClientByUserId(userId);
            var trainer = client == null ? _trainerRepository.GetTrainerByUserId(userId) : null;
            var routines = new List<Routine>();
            var routinesexercise = new List<Routinesexercise>();

            if (trainer != null)
            {
                routines = _routineRepository.GetByDni(trainer.Dnitrainer);
                if (routines == null || !routines.Any()) throw new NotFoundException("No routines found");
                routinesexercise = _routineExerciseRepository.GetByDni(trainer.Dnitrainer);
            }
            else if (client != null)
            {
                routines = _routineRepository.GetByDni(client.Dniclient);
                if (routines == null || !routines.Any()) throw new NotFoundException("No routines found for the client");
                routinesexercise = _routineExerciseRepository.GetByDni(client.Dniclient);
            }
            else
            {
                throw new NotFoundException("User not found");
            }

            // Asumimos que solo hay una rutina relevante para el usuario
            var routineDto = routines.FirstOrDefault() != null ? RoutineDto.Create(
                routines.First(),
                client != null ? $"{client.Firstname} {client.Lastname}" : null,
                client != null ? client.Birthdate.ToString("yyyy-MM-dd") : null
            ) : null;

            // Agrupar los ejercicios por día y evitar duplicados por exerciseId
            var exercisesDto = routinesexercise
                .Select(routinese => new RoutinesexerciseDto
                {
                    exercise = routinese.IdexerciseNavigation,
                    Breaktime = routinese.Breaktime,
                    Series = routinese.Series,
                    Day = routinese.Day
                })
                .ToList();


            // Estructura la respuesta
            return new
            {
                Routine = routineDto,
                Exercises = exercisesDto
            };
        }








    }
}


