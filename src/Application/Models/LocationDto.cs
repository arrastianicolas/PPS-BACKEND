using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class LocationDto
    {
        public int Idlocation { get; set; }

        public string Adress { get; set; }

        public string Name { get; set; }

        public int Isactive { get; set; }

        public ICollection<ShiftDto> Shifts { get; set; } = new List<ShiftDto>();

        public static LocationDto Create(Location location)
        {
            return new LocationDto
            {
                Idlocation = location.Idlocation,
                Adress = location.Adress,
                Name = location.Name,
                Isactive = location.Isactive,
                Shifts = location.Shifts.Select(ShiftDto.Create).ToList()
            };

        }
    }
}
