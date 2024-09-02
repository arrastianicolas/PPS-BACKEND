﻿using System;
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
        [Key, Column(Order = 0)]
        [ForeignKey("TrainerId")]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("ClientId")]
        public int ClientId { get; set; }
        public Client Client { get; set; }

        [Required]
        public string? Description { get; set; }
        [Key, Column(Order = 2)]

        public int NumeroCorrelativo { get; set; }

    }
}
