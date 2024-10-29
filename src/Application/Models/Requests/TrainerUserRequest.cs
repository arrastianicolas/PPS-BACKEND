using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class TrainerUserRequest
    {
        public TrainerRequest TrainerRequest { get; set; }
        public UserRequest UserRequest { get; set; }
    }
}
