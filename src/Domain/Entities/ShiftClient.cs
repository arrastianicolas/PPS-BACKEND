using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShiftClient
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Shift")]
        public int LocationId { get; set; }

        [Key, Column(Order = 2)]
        public DateTime Hour { get; set; }

        [Key, Column(Order = 3)]
        public DateTime Day { get; set; }

        public Shift Shift { get; set; }
    }

}
