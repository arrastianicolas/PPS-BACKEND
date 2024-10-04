using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Exceptions;
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
                Genre = clientRequest.Genre,
                Isactive = 1,
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
        public ClientUserDto GetUserById(int Iduser)
        {
            
            var user = _userRepository.GetById(Iduser);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            
            var clientUser = _clientRepository.GetClientByUserId(Iduser);
            if (clientUser == null)
            {
                throw new Exception("No se encontro al cliente.");
            }

            // Retorna el DTO combinado de usuario y cliente
            return ClientUserDtoMapper.Create(clientUser, user);
        }

        public void Delete(string clientDni)
        {
            var client = _clientRepository.GetByDni(clientDni);
            if (client == null)
            {
                throw new NotFoundException("No se encontro al cliente.");
            }
            client.Isactive = 0;
            _clientRepository.Update(client);
            
        }

    }
}
