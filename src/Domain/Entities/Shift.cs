using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Shift
    {
        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }
        [Required]
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        [Required]
        public Location Location { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime Hour { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Day { get; set; }

        [Required]
        public int PersonLimit { get; set; } = 30;

    }
}
