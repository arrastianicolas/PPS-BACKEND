using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class ShiftLocationDayRequest
    {
        public int locationId { get;  set; }
        public string day { get; set; }
    }
}
