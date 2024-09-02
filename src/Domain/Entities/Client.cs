using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Client : User
    {
        [Required]
        [ForeignKey("MembershipId")]
        public int MembershipId { get; set; }
        public Membership Membership { get; set; }

    }
}
