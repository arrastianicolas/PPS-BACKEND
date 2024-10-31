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

        public NutritionalPlanDto Create(string clientDni, string trainerDni)
        {
            var trainer = _trainerRepository.GetByDni(trainerDni) ?? throw new NotFoundException("Trainer not found");
            var client = _clientRepository.GetByDni(clientDni) ?? throw new NotFoundException("Trainer not found"); ;
            var trainerEmail = _userRepository.GetById(trainer.Iduser)?.Email;

            var nutritionalPlan = new Nutritionalplan
            {
                Dniclient = client.Dniclient,
                Dnitrainer = trainer.Dnitrainer,
                //IsPending = 1

                //Description = request.Description,
                //Breakfast = request.Breakfast,
                //Lunch = request.Lunch,
                //Dinner = request.Dinner,
                //Brunch = request.Brunch,
                //Snack = request.Snack
            };

            _nutritionalPlanRepository.Add(nutritionalPlan);

            _mailService.Send($"Nueva solicitud de plan nutricional disponible",
                              $"Hola {trainer.Firstname}, tiene una nueva petición de plan nutricional disponible en su panel. Por favor, inicie sesión para revisar y completar la solicitud.",
                              trainerEmail);


            return NutritionalPlanDto.Create(nutritionalPlan);
        }

        public void Update(int id, NutritionalPlanRequest request)
        {
            var plan = _nutritionalPlanRepository.GetById(id) ?? throw new Exception("Plan not found.");

            plan.Description = request.Description;
            plan.Breakfast = request.Breakfast;
            plan.Lunch = request.Lunch;
            plan.Dinner = request.Dinner;
            plan.Brunch = request.Brunch;
            plan.Snack = request.Snack;
            //plan.IsPending = 0

            _nutritionalPlanRepository.Update(plan);
        }

        public void Delete(int id)
        {
            var plan = _nutritionalPlanRepository.GetById(id) ?? throw new Exception("Plan not found.");
            plan.IsActive = 0;            
            _nutritionalPlanRepository.Update(plan);
        }
    }
}

        



