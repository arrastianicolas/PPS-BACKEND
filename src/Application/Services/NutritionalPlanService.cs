using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MercadoPago.Resource.User;
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
        private readonly IShiftRepository _shiftRepository;
        private readonly IShiftClientRepository _shiftClientRepository;

        public NutritionalPlanService(INutritionalPlanRepository nutritionalPlanRepository, IMailService mailService, ITrainerRepository trainerRepository, IClientRepository clientRepository, IUserRepository userRepository, IShiftClientRepository shiftClientRepository, IShiftRepository shiftRepository )
        {
            _nutritionalPlanRepository = nutritionalPlanRepository;
            _mailService = mailService;
            _trainerRepository = trainerRepository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _shiftClientRepository = shiftClientRepository;
            _shiftRepository = shiftRepository;
        }

        public List<NutritionalPlanDto> GetAll()
        {
            var plans = _nutritionalPlanRepository.Get();
            return plans.Select(NutritionalPlanDto.Create).ToList();
        }

        public List<NutritionalPlanDto> GetByDni(int userId)
        {

            var client = _clientRepository.GetClientByUserId(userId);
            var trainer = client == null ? _trainerRepository.GetTrainerByUserId(userId) : null;
            var plans = new List<Nutritionalplan>();

            if (trainer != null)            
                plans = _nutritionalPlanRepository.GetByDni(trainer.Dnitrainer);            
            else            
                plans = _nutritionalPlanRepository.GetByDni(client.Dniclient);
                   
            return plans.Select(NutritionalPlanDto.Create).ToList();
        }

        public NutritionalPlanDto Create(int clientId, NutritionalPlanClientRequest request)
        {
            var client = _clientRepository.GetClientByUserId(clientId) ?? throw new NotFoundException("Client not found");

            // Obtener trainer
            var lastShiftId = _shiftClientRepository.GetLastShiftId(client.Dniclient);
            var lastShift = _shiftRepository.GetById(lastShiftId);        
            if (lastShift == null) throw new NotFoundException("No shifts found for the client");
            var trainer = _trainerRepository.GetByDni(lastShift.Dnitrainer) ?? throw new NotFoundException("Trainer not found");
            var trainerEmail = _userRepository.GetById(trainer.Iduser)?.Email;

            var nutritionalPlan = new Nutritionalplan
            {
                Dniclient = client.Dniclient,
                Dnitrainer = trainer.Dnitrainer,                
                Description = request.Description,
                Weight = request.Weight,
                Height = request.Height,
                Status = "In Progress"
                
            };

            _nutritionalPlanRepository.Add(nutritionalPlan);

            _mailService.Send($"Nueva solicitud de plan nutricional disponible",
                              $"Hola {trainer.Firstname},\n\nTienes una nueva petición de plan nutricional disponible en su panel\n\n.",
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
            
            if (request.Status.Equals("Denied", StringComparison.OrdinalIgnoreCase))
            {
                plan.Status = "Denied";
                string message = $"Hola {client.Firstname},\n\nLamentamos informarte que tu solicitud de plan nutricional ha sido rechazada." + (request.Message != null ? $"\n\nMotivo del rechazo:\n{request.Message}" : "") + "\n\nSi tienes alguna pregunta o necesitas más detalles, por favor, contacta a tu entrenador.";
               _mailService.Send($"Plan nutricional rechazado", message, clientEmail);
            } else
            {                
               // Si hay otro plan activo se desactiva para solo dejar activo el nuevo
               var clientPlans = _nutritionalPlanRepository.GetByDni(plan.Dniclient);
               var clientPlanActive = clientPlans.FirstOrDefault(p => p.Status == "Enabled");
               if(clientPlanActive != null)
                {
                    clientPlanActive.Status = "Disabled";
                    _nutritionalPlanRepository.Update(clientPlanActive);
                }

                plan.Status = "Enabled";

                _mailService.Send($"Plan nutricional aceptado",
                  $"Hola {client.Firstname},\n\nTu plan nutricional ha sido aceptado y está disponible en tu panel.\n\nPor favor, ingresa al sistema para revisarlo y comenzar a seguirlo.",
                  clientEmail);
            }
            _nutritionalPlanRepository.Update(plan);
        }

        public void Delete(int id)
        {
            var plan = _nutritionalPlanRepository.GetById(id) ?? throw new NotFoundException("Plan not found.");
            plan.Status = "Disabled";            
            _nutritionalPlanRepository.Update(plan);
        }
    }
}

        



