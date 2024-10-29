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
        public int Id { get; set; }
        public string DniClient { get; set; }
        public string DniTrainer { get; set; }
        public string Description { get; set; }
        public string? Breakfast { get; set; }
        public string? Lunch { get; set; }
        public string? Dinner { get; set; }
        public string? Brunch { get; set; }
        public string? Snack { get; set; }

        public static NutritionalPlanDto Create(Nutritionalplan nutritionalPlan)
        {
            return new NutritionalPlanDto
            {
                Id = nutritionalPlan.Idnutritionalplan,
                DniClient = nutritionalPlan.Dniclient,
                DniTrainer = nutritionalPlan.Dnitrainer,
                Description = nutritionalPlan.Description,
                Breakfast = nutritionalPlan.Breakfast,
                Lunch = nutritionalPlan.Lunch,
                Dinner = nutritionalPlan.Dinner,
                Brunch = nutritionalPlan.Brunch,
                Snack = nutritionalPlan.Snack
            };
        }
    }
}
