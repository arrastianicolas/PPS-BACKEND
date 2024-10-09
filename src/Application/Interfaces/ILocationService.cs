using Application.Models.Requests;
using Application.Models;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILocationService
    {
        List<LocationDto> GetAll();
        List<LocationDto> GetActives();
        LocationDto CreateLocation(LocationRequest locationRequest);
        void UpdateLocation(LocationRequest locationRequest, int id);
        void DeleteLocation(int id);
        void AddShift(int shiftId, int locationId);      
        void RemoveShift(int shiftId, int locationId);
        
    }
}
