﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class LocationRequest
    {

        public string Adress { get; set; }

        public string Name { get; set; }

        public int Isactive { get; set; }

    }
}
