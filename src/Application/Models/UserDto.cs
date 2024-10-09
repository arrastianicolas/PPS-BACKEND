using Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        
        public string? Type { get; set; }
        

        public static UserDto Create(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Type = user.Type,
               

            };
        }

        public static List<UserDto> CreateList(IEnumerable<User> users)
        {
            return users.Select(Create).ToList();
        }
    }
}
