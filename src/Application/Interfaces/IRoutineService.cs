using Application.Models.Requests;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRoutineService
    {
        RoutineDto Create(RoutineClientRequest routineClientRequest, int userId);
        void Update(int idRoutina, List<RoutineTrainerRequest> request);
        public List<RoutineDto> GetAll();
        public List<RoutineDto> GetByDni(int userId);
        public void Delete(int id);
    }
}
