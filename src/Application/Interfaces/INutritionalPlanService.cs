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
        List<NutritionalPlanDto> GetByDni(int usertId);
        NutritionalPlanDto Create(int clientId, NutritionalPlanClientRequest request);
        void Update(int id, NutritionalPlanTrainerRequest request);
        void Delete(int id);
    }
}
