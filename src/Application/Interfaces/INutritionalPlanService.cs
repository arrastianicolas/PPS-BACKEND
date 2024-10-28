using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Models;
using Application.Models.Requests;

namespace Application.Interfaces
{
    public interface INutritionalPlanService
    {
        List<NutritionalPlanDto> GetAll();
        NutritionalPlanDto Create(NutritionalPlanRequest request);
        void Update(int id, NutritionalPlanRequest request);
        void Delete(int id);
    }
}
