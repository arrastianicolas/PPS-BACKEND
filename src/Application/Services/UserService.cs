using Application.Interfaces;
using Application.Models.Requests;
using Application.Models;
using Domain.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        
        public UserService(IUserRepository userRepository, IMailService mailService)
        {
            _userRepository = userRepository;
            _mailService = mailService;
        }

        public  List<UserWithDetailsDto> Get()
        {
            var users =  _userRepository.Get();

            if (users == null || !users.Any())
            {
                return new List<UserWithDetailsDto>();
            }
            _mailService.Send($"¡Bienvenido a Training Center, !",
            $"Hola ,\r\n\r\n¡Nos complace darte la bienvenida a Training Center! Estamos emocionados de que te unas a nuestra comunidad de entrenamiento y bienestar.\r\n\r\nEn Training Center, nos comprometemos a ayudarte a alcanzar tus metas, ya sea mejorar tu condición física, aumentar tu fuerza o simplemente mantener un estilo de vida saludable. No dudes en acercarte a cualquiera de nuestros entrenadores para recibir orientación personalizada.\r\n\r\nTu bienestar es nuestra prioridad, y estamos aquí para acompañarte en cada paso de tu camino hacia el éxito.\r\n\r\n¡Nos vemos en el gimnasio!\r\n\r\nAtentamente,\r\nEl equipo de Training Center", "trainingcentergrupo2@gmail.com");

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

        public string? RequestResetPassword(string email)
        {
            var user = _userRepository.GetByUserEmail(email) ?? throw new NotFoundException("Email is not valid");

            var random = new Random();
            var code = random.Next(100000, 999999).ToString();
            

            _mailService.Send(
                "Solicitud de restablecimiento de contraseña",
                $"¿Restablecer tu contraseña?\r\n\r\nSi solicitaste un restablecimiento de contraseña, usa el código de confirmación que aparece a continuación para completar el proceso. Si no solicitaste esto, puedes ignorar este correo electrónico\r\n\r\n{code}.",
                user.Email
                );

            return code;
        }

        public void ResetPassword(ResetPasswordRequest request)
        {
            var user = _userRepository.GetByUserEmail(request.Email) ?? throw new NotFoundException("User not found");
            user.Password = request.NewPassword;
            _userRepository.Update(user);

            _mailService.Send(
             "Contraseña restablecida con éxito",
             $"Hola,\r\n\r\nTe confirmamos que tu contraseña ha sido restablecida correctamente.\r\nSi no realizaste esta acción, por favor contacta con nuestro equipo de soporte inmediatamente.\r\n\r\nGracias por utilizar nuestros servicios.\r\nAtentamente,\r\nTraining Center.",
             user.Email
             );
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
