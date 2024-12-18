﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILocationRepository : IBaseRepository<Location>
    {
        List<Location> GetActives();
        Location? GetById(int id);
        Task<List<object>> GetClientsCountByLocationAsync();

    }
}
