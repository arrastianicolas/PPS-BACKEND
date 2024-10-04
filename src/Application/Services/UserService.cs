using Application.Interfaces;
using Application.Models.Requests;
using Application.Models;
using Domain.Interfaces;
using Infrastructure.TempModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public  List<UserWithDetailsDto> Get()
        {
            var users =  _userRepository.Get();

            if (users == null || !users.Any())
            {
                return new List<UserWithDetailsDto>();
            }

            return (List<UserWithDetailsDto>)users;
        }

        public UserDto GetUserById(int id)
        {
            var user =  _userRepository.GetById(id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var userDto = UserDto.Create(user);

            return userDto;
        }

        //public User CreateUser(UserRequest user)
        //{
        //    var users =  _userRepository.Get();

        //    if (users.Any(u => u.Email == user.Email) || users.Any(u => u.UserName == user.UserName))
        //    {
        //        throw new Exception("A user with the same email or username already exists.");
        //    }
        //    User newUser;

        //    if (user.UserType == "Client")
        //    {
        //        newUser = new Client
        //        {
        //            UserName = user.UserName,
        //            Email = user.Email,
        //            Password = user.Password,
        //            UserType = user.UserType,

        //        };
        //    }
        //    else if (user.UserType == "Trainer")
        //    {
        //        newUser = new Trainer
        //        {
        //            UserName = user.UserName,
        //            Email = user.Email,
        //            Password = user.Password,
        //            UserType = user.UserType,

        //        };
        //    }
        //    else if (user.UserType == "SysAdmin")
        //    {
        //        newUser = new SysAdmin
        //        {
                    
        //            Email = user.Email,
        //            Password = user.Password,
        //            Type = user.Type,

        //        };
        //    }
        //    else
        //    {
        //        throw new Exception("Invalid user type.");
        //    }
        //    return  _userRepository.Add(newUser);
        //}

        //public void DeleteUser(int id)
        //{
        //    var user = _userRepository.GetById(id);

        //    if (user == null)
        //    {
        //        throw new Exception("User not found.");
        //    }

        //     _userRepository.Remove(user);
        //}

        //public void UpdateUser(int id, UserRequest updatedUser)
        //{
        //    var user =  _userRepository.GetById(id);

        //    if (user == null)
        //    {
        //        throw new Exception("User not found.");
        //    }

        //    user.Email = updatedUser.Email;
        //    user.Password = updatedUser.Password;

        //     _userRepository.Update(user);
        //}

    }
}
