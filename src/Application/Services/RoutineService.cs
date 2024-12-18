﻿using Application.Models.Requests;
using Application.Models;
using Domain.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using System.Numerics;
using MercadoPago.Resource.User;

namespace Application.Services
{
    public class RoutineService : IRoutineService
    {
        private readonly IRoutineRepository _routineRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IShiftClientRepository _shiftClientRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly ITrainerRepository _trainerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IRoutineExerciseRepository _routineExerciseRepository;
        public RoutineService(IClientRepository clientRepository, IRoutineRepository routineRepository, IShiftClientRepository shiftClientRepository, IShiftRepository shiftRepository, ITrainerRepository trainerRepository, IUserRepository userRepository, IMailService mailService, IRoutineExerciseRepository routineExerciseRepository)
        {
            _routineRepository = routineRepository;
            _clientRepository = clientRepository;
            _shiftClientRepository = shiftClientRepository;
            _shiftRepository = shiftRepository;
            _trainerRepository = trainerRepository;
            _userRepository = userRepository;
            _mailService = mailService;
            _routineExerciseRepository = routineExerciseRepository;

        }
        public List<RoutineDto> GetAll()
        {
            var routines = _routineRepository.Get();


            return routines.Select(routine =>
            {
                var client = _clientRepository.GetByDni(routine.Dniclient);

                string clientName = $"{client.Firstname} {client.Lastname}";
                string clientBirthdate = $"{client.Birthdate}";

                return RoutineDto.Create(routine, clientName, clientBirthdate);
            }).ToList();
        }
        public List<RoutineDto> GetByDni(int userId)
        {

            var client = _clientRepository.GetClientByUserId(userId);
            var trainer = client == null ? _trainerRepository.GetTrainerByUserId(userId) : null;
            var routines = new List<Routine>();

            if (trainer != null)
                routines = _routineRepository.GetByDni(trainer.Dnitrainer);
            else
                routines = _routineRepository.GetByDni(client.Dniclient);

            return routines.Select(routine =>
            {
                var client = _clientRepository.GetByDni(routine.Dniclient);

                string clientName = $"{client.Firstname} {client.Lastname}";
                string clientBirthdate = $"{client.Birthdate}";

                return RoutineDto.Create(routine, clientName, clientBirthdate);
            }).ToList();
        }
        public RoutineDto Create(RoutineClientRequest routineClientRequest, int userId)
        {
            var client = _clientRepository.GetClientByUserId(userId);
            var lastShiftId = _shiftClientRepository.GetLastShiftId(client.Dniclient);
            var lastShift = _shiftRepository.GetById(lastShiftId);
            if (lastShift == null) throw new NotFoundException("No shifts found for the client");
            var trainer = _trainerRepository.GetByDni(lastShift.Dnitrainer) ?? throw new NotFoundException("Trainer not found");
            var trainerEmail = _userRepository.GetById(trainer.Iduser)?.Email;

            var routine = new Routine()
            {

                Dniclient = client.Dniclient,
                Dnitrainer = trainer.Dnitrainer,
                //DniTrainer = routineClientRequest.DniTrainer;
                Weight = routineClientRequest.Weight,
                Height = routineClientRequest.Height,
                Status = "In Progress",
                Description = routineClientRequest.Description,
                Days = routineClientRequest.Days
            };
            _routineRepository.Add(routine);
            _mailService.Send($"Nueva solicitud de rutina disponible",
                              $"Hola {trainer.Firstname},\n\nTienes una nueva petición de rutina disponible en su panel\n\n.",
                              trainerEmail);
            return RoutineDto.Create(routine);
        }
        public void Update(int idRoutina, List<RoutineTrainerRequest> request)
        {
            var routine = _routineRepository.GetById(idRoutina) ?? throw new NotFoundException("Routine not found.");
            
            

            try
            {

            foreach (var assigned in request)
            {
                var routineExercise = new Routinesexercise
                {
                    Idroutine = routine.Idroutine,
                    Idexercise = assigned.idExercise,
                    Breaktime = assigned.Breaktime,
                    Series = assigned.Series,
                    Day = assigned.Day
                };

                    
                
                _routineExerciseRepository.Add(routineExercise);
                    routine.Status = "Done";
                    _routineRepository.Update(routine);
                }
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }

        }
        public void ChangeStatusToDone(string dniClient)
        {
            try
            {
                _routineRepository.ChangeStatusToDone(dniClient);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Delete(int id)
        {
            var routine = _routineRepository.GetById(id) ?? throw new NotFoundException("routine not found.");
            routine.Status = "Disabled";
            _routineRepository.Update(routine);
        }

    }
}

