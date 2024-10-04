using Infrastructure.TempModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserWithDetailsDto
    {
        
        public Client Client { get; set; }
        public Trainer Trainer { get; set; }
    }

}
