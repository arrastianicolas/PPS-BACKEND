using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class RoutineClientRequest
    {
        public string Weight { get; set; }
        public string Height { get; set; }
        public string Description { get; set; }
        public int Days {  get; set; }

    }
}
