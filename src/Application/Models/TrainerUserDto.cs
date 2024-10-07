using Infrastructure.TempModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class TrainerUserDto
    {
        public TrainerDto TrainerDto { get; set; } = null!;
        public UserDto UserDto { get; set; } = null!;
    }

    public static class TrainerUserDtoMapper
    {
        public static TrainerUserDto Create(Trainer trainer, User user)
            {
                return new TrainerUserDto
                {
                    TrainerDto = TrainerDto.Create(trainer),
                    UserDto = UserDto.Create(user)
                };
            }
    }
    
}
