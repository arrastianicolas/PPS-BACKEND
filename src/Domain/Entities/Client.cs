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
        public int Document { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string? Name { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string? LastName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string? FirstName { get; set; }
        [Required]
        public int? PhoneNumber { get; set; }
        [Required]
        public string? BirthDay { get; set; }

    }
}
