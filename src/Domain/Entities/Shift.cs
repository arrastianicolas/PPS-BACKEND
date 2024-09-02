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
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("TrainerId")]
        public int TrainerId { get; set; }

        public Trainer Trainer { get; set; }
        [Key, Column(Order = 2)]
        [ForeignKey("LocationId")]
        public int LocationId { get; set; }

        public Location Location { get; set; }

        [Required]
        [DataType(DataType.Time)]
        
        public DateTime Hour { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Day { get; set; }
    }
}
