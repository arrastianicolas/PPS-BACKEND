using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Routine
    {
        
        public int CorrelativeNumber { get; set; }
        [Required]
        public string? Description { get; set; }

        [Required]
        [ForeignKey("TrainerId")]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }
        [Required]
        [ForeignKey("ClientId")]
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
