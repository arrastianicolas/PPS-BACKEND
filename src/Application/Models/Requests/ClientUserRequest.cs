using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class ClientUserRequest
    {
        public ClientRequest ClientRequest { get; set; }
        public UserRequest UserRequest { get; set; }
    }
}
