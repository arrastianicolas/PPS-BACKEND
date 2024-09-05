using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Trainer : User
    {
        [Required]
        public int FileNumber { get; set; }
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
