using Application.Interfaces;
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
    public class NutritionalPlanService : INutritionalPlanService
    {
       
        private readonly INutritionalPlanRepository _nutritionalPlanRepository;

        public NutritionalPlanService(INutritionalPlanRepository nutritionalPlanRepository)
        {
            _nutritionalPlanRepository = nutritionalPlanRepository;
        }

        public List<NutritionalPlanDto> GetAll()
        {
            var plans = _nutritionalPlanRepository.GetAll();
            return plans.Select(NutritionalPlanDto.Create).ToList();
        }

        public NutritionalPlanDto Create(NutritionalPlanRequest request)
        {
            var nutritionalPlan = new Nutritionalplan
            {
                Dniclient = request.DniClient,
                Dnitrainer = request.DniTrainer,
                Description = request.Description,
                Breakfast = request.Breakfast,
                Luch = request.Lunch,
                Dinner = request.Dinner,
                Brunch = request.Brunch,
                Snack = request.Snack
            };

            _nutritionalPlanRepository.Add(nutritionalPlan);
            return NutritionalPlanDto.Create(nutritionalPlan);
        }

        public void Update(int id, NutritionalPlanRequest request)
        {
            var plan = _nutritionalPlanRepository.GetById(id) ?? throw new Exception("Plan not found.");
            plan.Dniclient = request.DniClient;
            plan.Dnitrainer = request.DniTrainer;
            plan.Description = request.Description;
            plan.Breakfast = request.Breakfast;
            plan.Luch = request.Lunch;
            plan.Dinner = request.Dinner;
            plan.Brunch = request.Brunch;
            plan.Snack = request.Snack;

            _nutritionalPlanRepository.Update(plan);
        }

        public void Delete(int id)
        {
            var plan = _nutritionalPlanRepository.GetById(id) ?? throw new Exception("Plan not found.");
            _nutritionalPlanRepository.Delete(id);
        }
    }
}

        



