using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    //ver si es para admin debido a que admin puede crear un usuario de tipo client , trainer u otro admin
    //un usuario por default crea un usuario en base a un cliente 
    public class UserRequest
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; } 

    }
}
