using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Interfaces
{
    public interface INutritionalPlanRepository
    {
        List<Nutritionalplan> GetAll();
        Nutritionalplan? GetById(int id);
        Nutritionalplan Add(Nutritionalplan nutritionalPlan);
        Nutritionalplan Update(Nutritionalplan nutritionalPlan);
        void Delete(int id);
    }
}
