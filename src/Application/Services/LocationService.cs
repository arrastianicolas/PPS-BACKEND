using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IShiftRepository _shiftRepository;

        public LocationService(ILocationRepository locationRepository, IShiftRepository shiftRepository)
        {   
            _locationRepository = locationRepository;
            _shiftRepository = shiftRepository;
        }
        public List<LocationDto> GetAll()
        {
            var locations = _locationRepository.Get();
            return locations.Select(LocationDto.Create).ToList();
        }
        
        public List<LocationDto> GetActives()
        {
            var locations = _locationRepository.GetActives();
            return locations.Select(LocationDto.Create).ToList();
        }

        public LocationDto CreateLocation(LocationRequest locationRequest) 
        {
            var location = new Location()
            {
                Adress = locationRequest.Adress,
                Name = locationRequest.Name
            };

            _locationRepository.Add(location);
            return LocationDto.Create(location);
        }

        public void UpdateLocation(LocationRequest locationRequest, int id)
        {
            var location = _locationRepository.GetById(id) ?? throw new Exception("Location not found.");

            location.Adress = locationRequest.Adress;
            location.Name = locationRequest.Name;

            _locationRepository.Update(location);
        }

        public void DeleteLocation(int id)
        {
            var location = _locationRepository.GetById(id) ?? throw new Exception("Location not found.");
            location.Isactive = 0;
            _locationRepository.Update(location);
        }


        public void ChangeState(int locationId)
        {
            var location = _locationRepository.GetById(locationId) ?? throw new Exception("Location not found.");

            location.Isactive = location.Isactive == 1 ? 0 : 1;

            _locationRepository.Update(location);
        }

        public async Task<List<object>> GetClientsCountByLocationAsync()
        {
            return await _locationRepository.GetClientsCountByLocationAsync();
        }

    }
}
