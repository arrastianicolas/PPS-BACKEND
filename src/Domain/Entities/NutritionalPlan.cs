using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class NutritionalPlan
    {
        
        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        
        public int CorrelativeNumber { get; set; }

        [Required]
        public string? Description { get; set; }
    }

}
