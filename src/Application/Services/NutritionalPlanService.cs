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
    public class NutritionalPlanService : INutritionalPlanService
    {
       
        private readonly INutritionalPlanRepository _nutritionalPlanRepository;
        private readonly IMailService _mailService;
        private readonly ITrainerRepository _trainerRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;

        public NutritionalPlanService(INutritionalPlanRepository nutritionalPlanRepository, IMailService mailService, ITrainerRepository trainerRepository, IClientRepository clientRepository, IUserRepository userRepository)
        {
            _nutritionalPlanRepository = nutritionalPlanRepository;
            _mailService = mailService;
            _trainerRepository = trainerRepository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
        }

        public List<NutritionalPlanDto> GetAll()
        {
            var plans = _nutritionalPlanRepository.Get();
            return plans.Select(NutritionalPlanDto.Create).ToList();
        }

        public List<NutritionalPlanDto> GetByClientDni(int clientId)
        {
            var client = _clientRepository.GetClientByUserId(clientId) ?? throw new NotFoundException("Client not found");
            var plans = _nutritionalPlanRepository.GetByClientDni(client.Dniclient);
            return plans.Select(NutritionalPlanDto.Create).ToList();
        }

        public NutritionalPlanDto Create(int clientId, NutritionalPlanClientRequest request)
        {
            var client = _clientRepository.GetClientByUserId(clientId) ?? throw new NotFoundException("Client not found");
            
            string trainerDni = "12345678"; // obtener trainer
            var trainer = _trainerRepository.GetByDni(trainerDni) ?? throw new NotFoundException("Trainer not found");
            var trainerEmail = _userRepository.GetById(trainer.Iduser)?.Email;

            var nutritionalPlan = new Nutritionalplan
            {
                Dniclient = client.Dniclient,
                Dnitrainer = trainer.Dnitrainer,                
                Description = request.Description,
                IsPending = 1,
                Weight = request.Weight,
                Height = request.Height,
                IsActive = 0
                
            };

            _nutritionalPlanRepository.Add(nutritionalPlan);

            _mailService.Send($"Nueva solicitud de plan nutricional disponible",
                              $"Hola {trainer.Firstname},\n\nTienes una nueva petición de plan nutricional disponible en su panel\n\nPor favor, ingrese al sistema para revisar y completar la solicitud.",
                              trainerEmail);


            return NutritionalPlanDto.Create(nutritionalPlan);
        }

        public void Update(int id, NutritionalPlanTrainerRequest request)
        {
            var plan = _nutritionalPlanRepository.GetById(id) ?? throw new NotFoundException("Plan not found.");
            var client = _clientRepository.GetByDni(plan.Dniclient);
            string clientEmail = _userRepository.GetById(_clientRepository.GetByDni(plan.Dniclient).Iduser).Email;
            plan.Breakfast = request.Breakfast;
            plan.Lunch = request.Lunch;
            plan.Dinner = request.Dinner;
            plan.Brunch = request.Brunch;
            plan.Snack = request.Snack;
            plan.IsPending = 0;
            if (request.isActive == 0)
            {
                plan.IsActive = 0;
                string message = $"Hola {client.Firstname},\n\nLamentamos informarte que tu solicitud de plan nutricional ha sido rechazada." + (request.Message != null ? $"\n\nMotivo del rechazo:\n{request.Message}" : "") + "\n\nSi tienes alguna pregunta o necesitas más detalles, por favor, contacta a tu entrenador.";
               _mailService.Send($"Plan nutricional rechazado", message, clientEmail);
            } else
            {                
               // Si hay otro plan activo se desactiva para solo dejar activo el nuevo
               var clientPlans = _nutritionalPlanRepository.GetByClientDni(plan.Dniclient);
               var clientPlanActive = clientPlans.FirstOrDefault(p => p.IsActive == 1);
               if(clientPlanActive != null)
                {
                    clientPlanActive.IsActive = 0;
                    _nutritionalPlanRepository.Update(clientPlanActive);
                }

                plan.IsActive = 1;

                _mailService.Send($"Plan nutricional aceptado",
                  $"Hola {client.Firstname},\n\nTu plan nutricional ha sido aceptado y está disponible en tu panel.\n\nPor favor, ingresa al sistema para revisarlo y comenzar a seguirlo.",
                  clientEmail);
            }
            _nutritionalPlanRepository.Update(plan);
        }

        public void Delete(int id)
        {
            var plan = _nutritionalPlanRepository.GetById(id) ?? throw new NotFoundException("Plan not found.");
            plan.IsActive = 0;            
            _nutritionalPlanRepository.Update(plan);
        }
    }
}

        



