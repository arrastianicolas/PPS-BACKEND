using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Interfaces;
using Infrastructure.TempModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IMailService _mailService;

        public ClientService(IUserRepository userRepository, IClientRepository clientRepository, IMembershipRepository membershipRepository, IMailService mailService)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _membershipRepository = membershipRepository;
            _mailService = mailService;
        }

        public ClientDto CreateClient(ClientRequest clientRequest, UserRequest userRequest)
        {

            //_mailService.Send($"¡Bienvenido a Training Center, {clientRequest.Firstname}!",
            //    $"Hola {clientRequest.Firstname} {clientRequest.Lastname},\r\n\r\n¡Nos complace darte la bienvenida a Training Center! Estamos emocionados de que te unas a nuestra comunidad de entrenamiento y bienestar.\r\n\r\nEn Training Center, nos comprometemos a ayudarte a alcanzar tus metas, ya sea mejorar tu condición física, aumentar tu fuerza o simplemente mantener un estilo de vida saludable. No dudes en acercarte a cualquiera de nuestros entrenadores para recibir orientación personalizada.\r\n\r\nTu bienestar es nuestra prioridad, y estamos aquí para acompañarte en cada paso de tu camino hacia el éxito.\r\n\r\n¡Nos vemos en el gimnasio!\r\n\r\nAtentamente,\r\nEl equipo de Training Center",
            //    userRequest.Email);

            var membership = _membershipRepository.Get()
                .FirstOrDefault(m => m.Type == clientRequest.Typememberships);

            var user = new User
            {
                Email = userRequest.Email!,
                Password = userRequest.Password!,
                Type = "Client"
            };

            var createdUser = _userRepository.Add(user);

            var client = new Client
            {
                Dniclient = clientRequest.Dniclient,
                Firstname = clientRequest.Firstname,
                Lastname = clientRequest.Lastname,
                Birthdate = clientRequest.Birthdate,
                Phonenumber = clientRequest.Phonenumber,
                Typememberships = membership.Type,
                Startdatemembership = DateTime.Now,
                Statusmembership = "Activa",
                Iduser = createdUser.Id
            };
            _clientRepository.Add(client);

            return ClientDto.Create(client);
        }
        public void UpdateClient(int Iduser, ClientRequest clientRequest, UserRequest userRequest)
        {
            var user = _userRepository.GetById(Iduser);
            if (user == null)
            {
                throw new KeyNotFoundException($"No se encontró un usuario con el ID: {Iduser}");
            }

            user.Password = userRequest.Password;
            user.Email = userRequest.Email;

            var client = _clientRepository.GetClientByUserId(user.Id);
            if (client != null)
            {
                client.Firstname = clientRequest.Firstname;
                client.Lastname = clientRequest.Lastname;
                client.Phonenumber = clientRequest.Phonenumber;
                client.Birthdate = clientRequest.Birthdate;
            }

            _userRepository.Update(user);
            if (client != null)
            {
                _clientRepository.Update(client);
            }

        }
        public ClientUserDto GetClientById(int Iduser)
        {
            
            var user = _userRepository.GetById(Iduser);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            
            var clientUser = _clientRepository.GetClientByUserId(Iduser);
            if (clientUser == null)
            {
                throw new Exception("Cliente no encontrado para el usuario especificado");
            }

            // Retorna el DTO combinado de usuario y cliente
            return ClientUserDtoMapper.Create(clientUser, user);
        }


    }
}
