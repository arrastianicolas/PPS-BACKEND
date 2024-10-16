using Application.Models.Requests;
using Application.Models;
using Domain.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class RoutineService : IRoutineService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IRoutineRepository _routineRepository;
        public RoutineService (IClientRepository clientRepository, IRoutineRepository routineRepository)
        {
            _routineRepository = routineRepository;
            _clientRepository = clientRepository;
        }

        public RoutineDto Add(RoutineClientRequest routineClientRequest,int userId)
        {
            var client = _clientRepository.GetClientByUserId(userId);
           

            var routine = new Routine()
            {
               
                Dniclient = client.Dniclient,
                Dnitrainer = "34765432", //Cambiar cuando esté hecho el modulo de turnos
                //DniTrainer = routineClientRequest.DniTrainer;
                Weight = routineClientRequest.Weight,
                Height = routineClientRequest.Height,
                Status = "En Progreso",
                Description = routineClientRequest.Description,
                Days = routineClientRequest.Days
            };
            _routineRepository.Add(routine);
            return RoutineDto.Create(routine);
        }

        
    }
}
