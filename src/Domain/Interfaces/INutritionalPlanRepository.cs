﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Interfaces
{
    public interface INutritionalPlanRepository : IBaseRepository<Nutritionalplan>
    {
        List<Nutritionalplan> GetByDni(string dni);
    }
}
