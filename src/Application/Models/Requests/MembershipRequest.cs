using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class MembershipRequest
    {
        

        [Required]
        public string Type { get; set; } = null!;
        [Required]
        public float Price { get; set; }
        [Required]
        public string Description { get; set; } = null!;
    }
}
