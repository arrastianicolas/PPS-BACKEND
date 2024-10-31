using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class NutritionalPlanClientRequest
    {
        public string Description { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }

    }

}
