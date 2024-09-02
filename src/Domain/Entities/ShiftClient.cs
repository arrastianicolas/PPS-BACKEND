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
        [ForeignKey("ClientId")]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        [Key, Column(Order = 1)]

        [ForeignKey("ShiftId")]
        public int ShiftId { get; set; }
        public Shift Shift { get; set; }
    }
}
