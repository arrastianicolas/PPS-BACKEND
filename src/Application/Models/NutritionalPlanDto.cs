using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class NutritionalPlanDto
    {
        public int IdNutritionalPlan { get; set; }
        public string DniClient { get; set; }
        public string DniTrainer { get; set; }
        public string Description { get; set; }
        public string? Breakfast { get; set; }
        public string? Lunch { get; set; }
        public string? Dinner { get; set; }
        public string? Brunch { get; set; }
        public string? Snack { get; set; }
        public string Status { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string? ClientName { get; set; }
        public string? ClientBirthdate { get; set; }

        public static NutritionalPlanDto Create(Nutritionalplan nutritionalPlan, string? clientName = null, string? clientBirthdate = null)
        {
            return new NutritionalPlanDto
            {
                IdNutritionalPlan = nutritionalPlan.Idnutritionalplan,
                DniClient = nutritionalPlan.Dniclient,
                DniTrainer = nutritionalPlan.Dnitrainer,
                Description = nutritionalPlan.Description,
                Breakfast = nutritionalPlan.Breakfast,
                Lunch = nutritionalPlan.Lunch,
                Dinner = nutritionalPlan.Dinner,
                Brunch = nutritionalPlan.Brunch,
                Snack = nutritionalPlan.Snack,
                Status = nutritionalPlan.Status,
                Weight = nutritionalPlan.Weight,
                Height = nutritionalPlan.Height,
                ClientName = clientName,
                ClientBirthdate = clientBirthdate
            };
        }
    }
}