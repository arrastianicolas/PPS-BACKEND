using Application.Models;
using Application.Models.Requests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {

        List<UserWithDetailsDto> Get();
        UserDto GetUserById(int id);
        //User CreateUser(UserRequest user);
        //void DeleteUser(int id);
        //void UpdateUser(int id, UserRequest updatedUser);
    }
}
