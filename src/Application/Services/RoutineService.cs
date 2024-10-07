using Application.Models.Requests;
using Application.Models;
using Domain.Interfaces;
using Infrastructure.TempModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;

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
                Correlativenumber = routineClientRequest.Correlativenumber,
                Dniclient = client.Dniclient,
                //DniTrainer = routineClientRequest.DniTrainer;
                Weight = routineClientRequest.Weight,
                Height = routineClientRequest.Height,
                Status = "En Progreso",
                Description = routineClientRequest.Description
            };
            _routineRepository.Add(routine);
            return RoutineDto.Create(routine);
        }

        
    }
}
