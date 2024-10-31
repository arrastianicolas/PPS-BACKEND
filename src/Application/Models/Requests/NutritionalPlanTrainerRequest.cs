using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class NutritionalPlanTrainerRequest
    {
        public string? Breakfast { get; set; }
        public string? Lunch { get; set; }
        public string? Dinner { get; set; }
        public string? Brunch { get; set; }
        public string? Snack { get; set; }


        // Si el entrenador quiere rechazar el plan, puede hacerlo enviando isActive como 0 y un mensaje aclarando el porque fue rechazado
        public int? isActive { get; set; } = 1;
        public string? Message { get; set; }
    }
}
